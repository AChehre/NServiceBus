namespace NServiceBus.Saga
{
    using System;

    /// <summary>
    /// Defines the basic functionality of a persister for storing 
	/// and retrieving a saga.
    /// </summary>
    public interface ISagaPersister
    {
		/// <summary>
		/// Saves the saga entity to the persistence store.
        /// </summary>
        /// <param name="metadata">Metadata describing the saga being saved.</param>
		/// <param name="saga">The saga entity to save.</param>
        void Save(SagaMetadata metadata, IContainSagaData saga);

        /// <summary>
        /// Updates an existing saga entity in the persistence store.
        /// </summary>
        /// <param name="metadata">Metadata describing the saga being updated.</param>
        /// <param name="saga">The saga entity to updated.</param>
        void Update(SagaMetadata metadata, IContainSagaData saga);

		/// <summary>
		/// Gets a saga entity from the persistence store by its Id.
        /// </summary>
        /// <param name="metadata">Metadata describing the saga being retrieved.</param>
		/// <param name="sagaId">The Id of the saga entity to get.</param>
        TSagaData Get<TSagaData>(SagaMetadata metadata, Guid sagaId) where TSagaData : IContainSagaData;

        /// <summary>
        /// Looks up a saga entity by a given property.
        /// </summary>
        /// <param name="metadata">Metadata describing the saga being completed.</param>
        /// <param name="propertyName">From the data store, analyze this property.</param>
        /// <param name="propertyValue">From the data store, look for this value in the identified property.</param>
        TSagaData Get<TSagaData>(SagaMetadata metadata, string propertyName, object propertyValue) where TSagaData : IContainSagaData;

		/// <summary>
        /// Sets a saga as completed and removes it from the active saga list
		/// in the persistence store.
		/// </summary>
		/// <param name="metadata">Metadata describing the saga being completed.</param>
		/// <param name="saga">The saga to complete.</param>
        void Complete(SagaMetadata metadata, IContainSagaData saga);

        /// <summary>
        /// Implementers can initialize the persistence with the given meta model.
        /// </summary>
        /// <param name="model">The sagas meta model.</param>
        void Initialize(SagaMetaModel model);
    }

}
