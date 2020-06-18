using System;

namespace Sample.Application.ViewModel
{
    public class SampleEditViewModel
    {
        public int Id { get; set; }
        public bool Active { get; set; }
    }

    public class SampleListViewModel
    {
        public int Id { get; set; }
        public int UserCreationId { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Active { get; set; }
    }
}