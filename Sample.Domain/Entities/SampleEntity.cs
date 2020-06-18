namespace Sample.Domain.Entities
{
    public class SampleEntity : EntityBase
    {
        // This class only extend entity base properties

        public SampleEntity()
        {

        }

        public override bool IsValid()
        {
            return true;
        }
    }
}