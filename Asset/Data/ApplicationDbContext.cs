using Asset.Data.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Asset.Data
{
    public class ApplicationDbContext : IdentityDbContext<Patron>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<LibraryAsset> LibraryAssets { get; set; }
        public virtual DbSet<Patron> Patrons { get; set; }
        public virtual DbSet<Hold> Holds { get; set; }
        public virtual DbSet<HoldHistory> HoldHistories { get; set; }
        public virtual DbSet<LibraryCard> LibraryCards { get; set; }
        public virtual DbSet<Checkout> Checkouts { get; set; }
        public virtual DbSet<CheckoutHistory> CheckoutHistory { get; set; }
        public virtual DbSet<LibraryBranch> LibraryBranches { get; set; }
        public virtual DbSet<BranchHours> BranchHours { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<LibraryTopic> LibraryTopics { get; set; }
        public virtual DbSet<CheckoutPenalty> CheckoutPenalties { get; set; }
        public virtual DbSet<CheckoutRequest> CheckoutRequests { get; set; }
    }
}
