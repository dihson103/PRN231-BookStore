﻿using System;
using System.Collections.Generic;

namespace App_API.Models;

public partial class Book
{
    public int BookId { get; set; }

    public int? PubId { get; set; }

    public string? Title { get; set; }

    public string? Advance { get; set; }

    public double? Royalty { get; set; }

    public double? YtdSales { get; set; }

    public string? Notes { get; set; }

    public DateTime? PublishedDate { get; set; }

    public virtual ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();

    public virtual Publisher? Pub { get; set; }
}
