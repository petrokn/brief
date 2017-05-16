﻿namespace brief.Library.Entities
{
    using System;
    using System.Collections.Generic;

    public class Book
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual IList<Edition> Editionss { get; set; }
        public virtual IList<Cover> Covers { get; set; }
        public virtual IList<Publisher> Publishers { get; set; }
    }
}
