namespace SchoolManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationDate = c.DateTime(),
                        StudentApplicationId = c.Int(nullable: false),
                        PreviousSchool = c.String(nullable: false),
                        PreviousSchoolAddrs = c.String(nullable: false),
                        PreviousSchoolDocument = c.Binary(),
                        Certificate = c.Binary(),
                        CertifiedID = c.Binary(),
                        HomeAddress = c.Binary(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StudentApplications", t => t.StudentApplicationId, cascadeDelete: true)
                .Index(t => t.StudentApplicationId);
            
            CreateTable(
                "dbo.StudentApplications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Surname = c.String(nullable: false),
                        DOB = c.DateTime(nullable: false),
                        NID = c.String(nullable: false),
                        Gender = c.String(nullable: false),
                        HomeLanguage = c.String(nullable: false),
                        Race = c.String(),
                        Email = c.String(nullable: false),
                        GuardianEmail = c.String(),
                        GuardianName = c.String(),
                        PresentAddress = c.String(nullable: false),
                        ParmanentAddress = c.String(nullable: false),
                        Religion = c.String(nullable: false),
                        creator = c.String(),
                        StudentNumber = c.String(),
                        ClassNameId = c.Int(nullable: false),
                        Status = c.String(),
                        subject = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClassNames", t => t.ClassNameId, cascadeDelete: true)
                .Index(t => t.ClassNameId);
            
            CreateTable(
                "dbo.ClassNames",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AssignTeacheToClasses",
                c => new
                    {
                        TeacherClassId = c.Int(nullable: false, identity: true),
                        TeacherId = c.Int(nullable: false),
                        ClassNameId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TeacherClassId)
                .ForeignKey("dbo.ClassNames", t => t.ClassNameId, cascadeDelete: true)
                .ForeignKey("dbo.Teachers", t => t.TeacherId, cascadeDelete: true)
                .Index(t => t.TeacherId)
                .Index(t => t.ClassNameId);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                        NID = c.String(),
                        Email = c.String(),
                        Gender = c.String(),
                        PhoneNumber = c.String(),
                        HomeAddress = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WorkSchedules",
                c => new
                    {
                        scheduleId = c.Int(nullable: false, identity: true),
                        scheduleStartDate = c.DateTime(nullable: false),
                        scheduleEndDate = c.DateTime(nullable: false),
                        ThemeColor = c.String(),
                        archived = c.Boolean(nullable: false),
                        classRoomId = c.Int(nullable: false),
                        staffMemberId = c.Int(nullable: false),
                        ClassName_Id = c.Int(),
                        teachers_Id = c.Int(),
                    })
                .PrimaryKey(t => t.scheduleId)
                .ForeignKey("dbo.ClassNames", t => t.ClassName_Id)
                .ForeignKey("dbo.Teachers", t => t.teachers_Id)
                .Index(t => t.ClassName_Id)
                .Index(t => t.teachers_Id);
            
            CreateTable(
                "dbo.ClassFees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FeeTypeId = c.Int(nullable: false),
                        ClassNameId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClassNames", t => t.ClassNameId, cascadeDelete: true)
                .ForeignKey("dbo.FeeTypes", t => t.FeeTypeId, cascadeDelete: true)
                .Index(t => t.FeeTypeId)
                .Index(t => t.ClassNameId);
            
            CreateTable(
                "dbo.FeeTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        FeeAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StudentClasses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ClassNameId = c.Int(nullable: false),
                        SectionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClassNames", t => t.ClassNameId, cascadeDelete: true)
                .ForeignKey("dbo.Sections", t => t.SectionId, cascadeDelete: true)
                .Index(t => t.ClassNameId)
                .Index(t => t.SectionId);
            
            CreateTable(
                "dbo.Sections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Code = c.String(nullable: false),
                        Subject_Credit = c.Double(nullable: false),
                        SubjectAssignTo = c.String(),
                        Theory = c.Int(nullable: false),
                        Mcq = c.Int(nullable: false),
                        Practical = c.Int(nullable: false),
                        ClassNameId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClassNames", t => t.ClassNameId, cascadeDelete: true)
                .Index(t => t.ClassNameId);
            
            CreateTable(
                "dbo.AttendanceRecords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentName = c.String(),
                        StudentSurname = c.String(),
                        Subject = c.String(),
                        Grade = c.String(),
                        NumberAbsent = c.Int(nullable: false),
                        NumberPresent = c.Int(nullable: false),
                        GuardianEmail = c.String(),
                        Attendance_Date = c.String(),
                        Attendance_status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Attendances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentName = c.String(),
                        StudentSurname = c.String(),
                        Subject = c.String(),
                        Grade = c.String(),
                        NumberAbsent = c.Int(nullable: false),
                        NumberPresent = c.Int(nullable: false),
                        GuardianEmail = c.String(),
                        Attendance_Date = c.String(),
                        Attendance_status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ThemeColors",
                c => new
                    {
                        colorID = c.Int(nullable: false, identity: true),
                        colorName = c.String(),
                        archived = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.colorID);
            
            CreateTable(
                "dbo.Guardians",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Surname = c.String(),
                        NID = c.String(),
                        Phone = c.String(nullable: false),
                        Email = c.String(),
                        HomeAddress = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GuardianTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Meetings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.String(),
                        Time = c.String(),
                        Venue = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.MeetingTeachers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TeacherID = c.Int(nullable: false),
                        MeetingID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        StationaryID = c.Int(nullable: false),
                        CollectOrDeliver = c.String(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Stationaries", t => t.StationaryID, cascadeDelete: true)
                .Index(t => t.StationaryID);
            
            CreateTable(
                "dbo.Stationaries",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Quantity = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StudentID = c.Int(nullable: false),
                        Term = c.String(),
                        Mark = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Subject = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Students", t => t.StudentID, cascadeDelete: true)
                .Index(t => t.StudentID);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        studentapp = c.Int(nullable: false),
                        Name = c.String(),
                        Surname = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        Gender = c.String(),
                        Email = c.String(),
                        PresentAddress = c.String(),
                        deposit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tuition = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BalanceDue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserName = c.String(),
                        NID = c.String(),
                        subject = c.String(),
                        Grade = c.String(),
                        StudentNumber = c.String(),
                        GuardianName = c.String(),
                        Status = c.String(),
                        date = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Residences",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        Rooms = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ResidenceStudents",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ResidenceID = c.Int(nullable: false),
                        StudentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ResidenceTypes",
                c => new
                    {
                        ResidenceTypeId = c.Int(nullable: false, identity: true),
                        ResidenceTypeName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ResidenceTypeId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Terms",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Reports", "StudentID", "dbo.Students");
            DropForeignKey("dbo.Orders", "StationaryID", "dbo.Stationaries");
            DropForeignKey("dbo.StudentApplications", "ClassNameId", "dbo.ClassNames");
            DropForeignKey("dbo.Subjects", "ClassNameId", "dbo.ClassNames");
            DropForeignKey("dbo.StudentClasses", "SectionId", "dbo.Sections");
            DropForeignKey("dbo.StudentClasses", "ClassNameId", "dbo.ClassNames");
            DropForeignKey("dbo.ClassFees", "FeeTypeId", "dbo.FeeTypes");
            DropForeignKey("dbo.ClassFees", "ClassNameId", "dbo.ClassNames");
            DropForeignKey("dbo.WorkSchedules", "teachers_Id", "dbo.Teachers");
            DropForeignKey("dbo.WorkSchedules", "ClassName_Id", "dbo.ClassNames");
            DropForeignKey("dbo.AssignTeacheToClasses", "TeacherId", "dbo.Teachers");
            DropForeignKey("dbo.AssignTeacheToClasses", "ClassNameId", "dbo.ClassNames");
            DropForeignKey("dbo.ApplicationDocuments", "StudentApplicationId", "dbo.StudentApplications");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Reports", new[] { "StudentID" });
            DropIndex("dbo.Orders", new[] { "StationaryID" });
            DropIndex("dbo.Subjects", new[] { "ClassNameId" });
            DropIndex("dbo.StudentClasses", new[] { "SectionId" });
            DropIndex("dbo.StudentClasses", new[] { "ClassNameId" });
            DropIndex("dbo.ClassFees", new[] { "ClassNameId" });
            DropIndex("dbo.ClassFees", new[] { "FeeTypeId" });
            DropIndex("dbo.WorkSchedules", new[] { "teachers_Id" });
            DropIndex("dbo.WorkSchedules", new[] { "ClassName_Id" });
            DropIndex("dbo.AssignTeacheToClasses", new[] { "ClassNameId" });
            DropIndex("dbo.AssignTeacheToClasses", new[] { "TeacherId" });
            DropIndex("dbo.StudentApplications", new[] { "ClassNameId" });
            DropIndex("dbo.ApplicationDocuments", new[] { "StudentApplicationId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Terms");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ResidenceTypes");
            DropTable("dbo.ResidenceStudents");
            DropTable("dbo.Residences");
            DropTable("dbo.Students");
            DropTable("dbo.Reports");
            DropTable("dbo.Stationaries");
            DropTable("dbo.Orders");
            DropTable("dbo.MeetingTeachers");
            DropTable("dbo.Meetings");
            DropTable("dbo.GuardianTypes");
            DropTable("dbo.Guardians");
            DropTable("dbo.ThemeColors");
            DropTable("dbo.Attendances");
            DropTable("dbo.AttendanceRecords");
            DropTable("dbo.Subjects");
            DropTable("dbo.Sections");
            DropTable("dbo.StudentClasses");
            DropTable("dbo.FeeTypes");
            DropTable("dbo.ClassFees");
            DropTable("dbo.WorkSchedules");
            DropTable("dbo.Teachers");
            DropTable("dbo.AssignTeacheToClasses");
            DropTable("dbo.ClassNames");
            DropTable("dbo.StudentApplications");
            DropTable("dbo.ApplicationDocuments");
        }
    }
}
