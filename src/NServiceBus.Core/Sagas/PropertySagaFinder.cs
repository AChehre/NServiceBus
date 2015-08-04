namespace NServiceBus.Saga
{
    using System;
    using NServiceBus.ObjectBuilder;

    /// <summary>
    /// Finds the given type of saga by looking it up based on the given property.
    /// </summary>
    class PropertySagaFinder<TSagaData> : SagaFinder where TSagaData : IContainSagaData
    {
        ISagaPersister sagaPersister;
        SagaMetaModel sagaMetaModel;

        public PropertySagaFinder(ISagaPersister sagaPersister, SagaMetaModel sagaMetaModel)
        {
            this.sagaPersister = sagaPersister;
            this.sagaMetaModel = sagaMetaModel;
        }

        internal override IContainSagaData Find(IBuilder builder,SagaFinderDefinition finderDefinition, object message)
        {
            var propertyAccessor = (Func<object,object>)finderDefinition.Properties["property-accessor"];
            var propertyValue = propertyAccessor(message);

            var sagaPropertyName = (string)finderDefinition.Properties["saga-property-name"];
            var sagaMetadata = sagaMetaModel.FindByEntity(typeof(TSagaData));

            if (sagaPropertyName.ToLower() == "id")
            {
                return sagaPersister.Get<TSagaData>(sagaMetadata, (Guid)propertyValue);
            }

            return sagaPersister.Get<TSagaData>(sagaMetadata, sagaPropertyName, propertyValue);
        }
    }
}
