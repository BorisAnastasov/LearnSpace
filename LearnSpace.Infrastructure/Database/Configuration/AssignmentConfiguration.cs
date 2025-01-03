using LearnSpace.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static LearnSpace.Common.Constants;

namespace LearnSpace.Infrastructure.Database.Configuration
{
    public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.HasData(SeedAssignments());
        }

        private static List<Assignment> SeedAssignments() 
        {
            var assignments = new List<Assignment>();

            //1 - Geometry
            var assignment = new Assignment()
            { 
                Id = 1,
                Title = "Geometric Foundations: Shapes, Angles, and Proofs",
                Description = "Test your understanding of the core concepts of geometry, including identifying shapes, calculating angles, and writing geometric proofs. This assessment covers both basic and advanced topics to challenge your spatial reasoning and problem-solving skills.",
                DueDate = DateTime.ParseExact("12/25/2024", DateFormat, System.Globalization.CultureInfo.InvariantCulture),
                CourseId = 1,
            };

            assignments.Add(assignment);

            //2 - Geometry
            assignment = new Assignment()
            {
                Id = 2,
                Title = "The Geometry Challenge: Area, Volume, and Theorems",
                Description = "This test focuses on applying geometric theorems to calculate areas, volumes, and properties of 2D and 3D shapes. Ideal for students ready to showcase their analytical abilities and mastery of geometric principles.",
                DueDate = DateTime.ParseExact("12/10/2024", DateFormat, System.Globalization.CultureInfo.InvariantCulture),
                CourseId = 1,
            };

            assignments.Add(assignment);

            //3 - Algebra
            assignment = new Assignment()
            {
                Id = 3,
                Title = "Algebra Essentials: Equations, Inequalities, and Graphs",
                Description = "Test your mastery of foundational algebra concepts, including solving equations, working with inequalities, and graphing linear and quadratic functions. Perfect for sharpening your analytical and computational skills.",
                DueDate = DateTime.ParseExact("12/26/2024", DateFormat, System.Globalization.CultureInfo.InvariantCulture),
                CourseId = 2,
            };

            assignments.Add(assignment);

            //4 - Algebra
            assignment = new Assignment()
            {
                Id = 4,
                Title = "Algebra in Action: Systems, Polynomials, and Word Problems",
                Description = "Put your algebra knowledge to the test with questions covering systems of equations, polynomial operations, and real-world applications. Designed to evaluate both theoretical understanding and practical problem-solving ability.",
                DueDate = DateTime.ParseExact("12/09/2024", DateFormat, System.Globalization.CultureInfo.InvariantCulture),
                CourseId = 2,
            };

            assignments.Add(assignment);

            //5 - Modern History
            assignment = new Assignment()
            {
                Id = 5,
                Title = "Modern History Milestones: Revolutions, Wars, and Transformations",
                Description = "Assess your understanding of key events in modern history, from political revolutions to world wars and the social transformations that shaped the 19th and 20th centuries. Test your knowledge of causes, consequences, and global impacts.",
                DueDate = DateTime.ParseExact("12/27/2024", DateFormat, System.Globalization.CultureInfo.InvariantCulture),
                CourseId = 3,
            };

            assignments.Add(assignment);

            //6 - Modern History
            assignment = new Assignment()
            {
                Id = 6,
                Title = "The Twentieth Century: Conflict, Progress, and Globalization",
                Description = "Examine pivotal events and trends of the 20th century, from world wars to the civil rights movement and globalization. This test evaluates your understanding of historical causes, key figures, and lasting legacies.",
                DueDate = DateTime.ParseExact("12/02/2024", DateFormat, System.Globalization.CultureInfo.InvariantCulture),
                CourseId = 3,
            };

            assignments.Add(assignment);

            //7 - Ancient History
            assignment = new Assignment()
            {
                Id = 7,
                Title = "Ancient Civilizations: Foundations of the Modern World",
                Description = "Explore the rise and fall of ancient civilizations, from Mesopotamia to Rome. This test covers key developments in governance, culture, and innovation that shaped early human history.",
                DueDate = DateTime.ParseExact("12/28/2024", DateFormat, System.Globalization.CultureInfo.InvariantCulture),
                CourseId = 4,
            };

            assignments.Add(assignment);

            //8 - Ancient History
            assignment = new Assignment()
            {
                Id = 8,
                Title = "Empires and Myths: A Journey Through Ancient History",
                Description = "Dive into the fascinating world of ancient empires, legendary battles, and enduring myths. This assessment evaluates your knowledge of ancient societies, their achievements, and their lasting influence on humanity.",
                DueDate = DateTime.ParseExact("12/06/2024", DateFormat, System.Globalization.CultureInfo.InvariantCulture),
                CourseId = 4,
            };

            assignments.Add(assignment);

            //9 - Middle Ages
            assignment = new Assignment()
            {
                Id = 9,
                Title = "The Middle Ages: Feudalism, Faith, and Fiefdoms",
                Description = "Test your knowledge of the medieval period, from the structure of feudal society to the influence of the Church and the everyday lives of people in the Middle Ages. Explore the events and ideas that defined this era.",
                DueDate = DateTime.ParseExact("12/25/2024", DateFormat, System.Globalization.CultureInfo.InvariantCulture),
                CourseId = 5,
            };

            assignments.Add(assignment);

            //10 - Middle Ages
            assignment = new Assignment()
            {
                Id = 10,
                Title = "Medieval History: Crusades, Kingdoms, and Culture",
                Description = "This test examines the major events and themes of the Middle Ages, including the Crusades, the growth of monarchies, and the cultural and intellectual achievements that emerged from this dynamic period.",
                DueDate = DateTime.ParseExact("12/14/2024", DateFormat, System.Globalization.CultureInfo.InvariantCulture),
                CourseId = 5,
            };

            assignments.Add(assignment);

            return assignments;
        }
    }
}
