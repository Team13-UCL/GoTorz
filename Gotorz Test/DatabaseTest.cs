using GoTorz;
using GoTorz.Components.Pages.Login;
using GoTorz.Components.Pages.PackagePages;
using GoTorz.Data;
using GoTorz.Model;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace Gotorz_Test
{

    public class DatabaseTest
    {
        private Mock<DbSet<T>> CreateMockDbSet<T>(IQueryable<T> data) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            //•	Provider: Bruges til at udføre forespørgsler (queries).
            //•	Expression: Repræsenterer forespørgslen som en udtrykstræ(expression tree).
            //•	ElementType: Angiver typen af elementer i samlingen.
            //•	GetEnumerator: Gør det muligt at iterere over dataene.
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }

        private Mock<GoTorzContext> CreateMockContext()
        {
            // Sample data for the Package DbSet
            var packageData = new List<Package>
            {
                new Package { PlaneId = 1, HotelId = 1, ReturnPlaneID = 1, PackagePrice = 500.00m },
                new Package { PlaneId = 2, HotelId = 2, ReturnPlaneID = 2, PackagePrice = 700.00m }
            }.AsQueryable();

            var mockPackageSet = CreateMockDbSet(packageData);

            // Mock the GoTorzContext
            var mockContext = new Mock<GoTorzContext>(new DbContextOptions<GoTorzContext>());
            mockContext.Setup(c => c.Package).Returns(mockPackageSet.Object);

            return mockContext;
        }

        [Fact]
        public void CanAddPackage()
        {
            // Arrange
            var mockContext = CreateMockContext();
            var newPackage = new Package
            {
                PlaneId = 3,
                HotelId = 3,
                ReturnPlaneID = 3,
                PackagePrice = 800.00m
            };

            var mockPackageSet = mockContext.Object.Package;

            // Act
            mockPackageSet.Add(newPackage);

            // Assert
            mockContext.Verify(c => c.Package.Add(It.IsAny<Package>()), Times.Once);
        }

        [Fact]
        public void CanReadPackage()
        {
            // Arrange
            var mockContext = CreateMockContext();

            // Act
            var package = mockContext.Object.Package.FirstOrDefault(p => p.PlaneId == 1);

            // Assert
            Assert.NotNull(package);
            Assert.Equal(500.00m, package.PackagePrice);
        }

        [Fact]
        public void CanUpdatePackage()
        {
            // Arrange
            var mockContext = CreateMockContext();
            var packageToUpdate = mockContext.Object.Package.FirstOrDefault(p => p.PlaneId == 1);

            // Act
            packageToUpdate.PackagePrice = 600.00m;

            // Assert
            Assert.Equal(600.00m, packageToUpdate.PackagePrice);
        }

        [Fact]
        public void CanDeletePackage()
        {
            // Arrange
            var mockContext = CreateMockContext();
            var packageToDelete = mockContext.Object.Package.FirstOrDefault(p => p.PlaneId == 1);

            var mockPackageSet = mockContext.Object.Package;

            // Act
            mockPackageSet.Remove(packageToDelete);

            // Assert
            mockContext.Verify(c => c.Package.Remove(It.IsAny<Package>()), Times.Once);
        }
    }
}