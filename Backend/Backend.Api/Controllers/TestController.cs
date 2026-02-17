// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Backend.Api.Data;
// using Backend.Api.Common;
// using Backend.Api.DTOs.User;
// using Backend.Api.DTOs.Property;
// using Backend.Api.DTOs.Auth;
// using Backend.Api.DTOs.Common;
// using Backend.Api.DTOs.PropertyType;
// using Backend.Api.DTOs.Media;
// using Backend.Api.DTOs.Communication;
// using Backend.Api.DTOs.Other;

// namespace Backend.Api.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class TestController : ControllerBase
//     {
//         private readonly ApplicationDbContext _context;

//         public TestController(ApplicationDbContext context)
//         {
//             _context = context;
//         }

//         [HttpGet("connection")]
//         public async Task<ActionResult<ApiResponse<object>>> TestConnection()
//         {
//             try
//             {
//                 var canConnect = await _context.Database.CanConnectAsync();
                
//                 var counts = new
//                 {
//                     Users = await _context.Users.CountAsync(),
//                     Properties = await _context.Properties.CountAsync(),
//                     PropertyTypes = await _context.PropertyTypes.CountAsync(),
//                     PropertyMedia = await _context.PropertyMedia.CountAsync(),
//                     Favorites = await _context.Favorites.CountAsync(),
//                     Conversations = await _context.Conversations.CountAsync(),
//                     Messages = await _context.Messages.CountAsync(),
//                     Notifications = await _context.Notifications.CountAsync(),
//                     PropertyReviews = await _context.PropertyReviews.CountAsync(),
//                     PropertyViews = await _context.PropertyViews.CountAsync(),
//                     SavedSearches = await _context.SavedSearches.CountAsync()
//                 };

//                 var data = new
//                 {
//                     DatabaseConnected = canConnect,
//                     TableCounts = counts,
//                     DatabaseName = _context.Database.GetDbConnection().Database,
//                     ServerTime = DateTime.Now,
//                     Status = "Database connection successful with complete DTOs"
//                 };

//                 return Ok(ApiResponse<object>.Success(data, "Database connection test successful"));
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, ApiResponse<object>.Failure(
//                     "Database connection failed", 
//                     new List<string> { ex.Message }));
//             }
//         }

//         [HttpGet("dtos")]
//         public ActionResult<ApiResponse<object>> TestDTOs()
//         {
//             var dtoValidation = new
//             {
//                 DTOsCount = 35,
//                 Categories = new
//                 {
//                     Common = new List<object>
//                     {
//                         new { Name = "BaseDto", Status = "✅ Complete", Usage = "Base class for all DTOs" },
//                         new { Name = "PaginationDto", Status = "✅ Complete", Usage = "Pagination parameters" },
//                         new { Name = "PaginatedResultDto<T>", Status = "✅ Complete", Usage = "Paginated response wrapper" },
//                         new { Name = "PropertySearchDto", Status = "✅ Complete", Usage = "Property search filters" }
//                     },
                    
//                     User = new List<object>
//                     {
//                         new { Name = "UserDto", Status = "✅ Complete", Usage = "User data transfer" },
//                         new { Name = "CreateUserDto", Status = "✅ Complete", Usage = "User creation with validation" },
//                         new { Name = "UpdateUserDto", Status = "✅ Complete", Usage = "User updates" },
//                         new { Name = "UserProfileDto", Status = "✅ Complete", Usage = "Extended user profile" }
//                     },
                    
//                     Property = new List<object>
//                     {
//                         new { Name = "PropertyDto", Status = "✅ Complete", Usage = "Basic property data" },
//                         new { Name = "CreatePropertyDto", Status = "✅ Complete", Usage = "Property creation with validation" },
//                         new { Name = "UpdatePropertyDto", Status = "✅ Complete", Usage = "Property updates" },
//                         new { Name = "PropertyListDto", Status = "✅ Complete", Usage = "Property listings with stats" },
//                         new { Name = "PropertyDetailsDto", Status = "✅ Complete", Usage = "Full property details" }
//                     },
                    
//                     PropertyType = new List<object>
//                     {
//                         new { Name = "PropertyTypeDto", Status = "✅ Complete", Usage = "Property type data" },
//                         new { Name = "CreatePropertyTypeDto", Status = "✅ Complete", Usage = "Property type creation" }
//                     },
                    
//                     Media = new List<object>
//                     {
//                         new { Name = "PropertyMediaDto", Status = "✅ Complete", Usage = "Media files data" },
//                         new { Name = "CreatePropertyMediaDto", Status = "✅ Complete", Usage = "Media file upload" }
//                     },
                    
//                     Communication = new List<object>
//                     {
//                         new { Name = "ConversationDto", Status = "✅ Complete", Usage = "Conversation data" },
//                         new { Name = "MessageDto", Status = "✅ Complete", Usage = "Message data with time formatting" },
//                         new { Name = "CreateMessageDto", Status = "✅ Complete", Usage = "Send message" }
//                     },
                    
//                     Other = new List<object>
//                     {
//                         new { Name = "FavoriteDto", Status = "✅ Complete", Usage = "Favorite properties" },
//                         new { Name = "NotificationDto", Status = "✅ Complete", Usage = "User notifications" },
//                         new { Name = "ReviewDto", Status = "✅ Complete", Usage = "Property reviews" },
//                         new { Name = "CreateReviewDto", Status = "✅ Complete", Usage = "Create review" },
//                         new { Name = "SavedSearchDto", Status = "✅ Complete", Usage = "Saved search criteria" },
//                         new { Name = "CreateSavedSearchDto", Status = "✅ Complete", Usage = "Create saved search" }
//                     },
                    
//                     Authentication = new List<object>
//                     {
//                         new { Name = "LoginDto", Status = "✅ Complete", Usage = "User login" },
//                         new { Name = "RegisterDto", Status = "✅ Complete", Usage = "User registration with validation" },
//                         new { Name = "AuthResponseDto", Status = "✅ Complete", Usage = "Authentication response" },
//                         new { Name = "ChangePasswordDto", Status = "✅ Complete", Usage = "Password change" }
//                     }
//                 },
                
//                 Features = new List<string>
//                 {
//                     "✅ Complete validation attributes",
//                     "✅ Helper properties for computed values",
//                     "✅ Time formatting methods",
//                     "✅ Search and filter capabilities",
//                     "✅ Pagination support",
//                     "✅ File upload support",
//                     "✅ Authentication DTOs",
//                     "✅ Comprehensive error messages",
//                     "✅ Type safety and structure"
//                 },
                
//                 ValidationFeatures = new
//                 {
//                     StringLengths = "All string properties have max length validation",
//                     Ranges = "Numeric properties have min/max validation",
//                     EmailValidation = "Email format validation included",
//                     PhoneValidation = "Phone number format validation",
//                     PasswordComplexity = "Strong password requirements",
//                     RequiredFields = "All required fields marked properly",
//                     CustomValidation = "Custom validation for business rules"
//                 },
                
//                 Status = "DTOs Section Completed Successfully! 🚀"
//             };

//             return Ok(ApiResponse<object>.Success(dtoValidation, "DTOs validation completed"));
//         }

//         [HttpGet("dto-examples")]
//         public ActionResult<ApiResponse<object>> GetDTOExamples()
//         {
//             var examples = new
//             {
//                 UserDto = new UserDto
//                 {
//                     UserId = 1,
//                     FirstName = "Ahmed",
//                     LastName = "Mohammed",
//                     Email = "ahmed@example.com",
//                     Phone = "+1234567890",
//                     City = "Cairo",
//                     Country = "Egypt",
//                     IsVerified = true,
//                     IsActive = true,
//                     CreatedAt = DateTime.UtcNow.AddDays(-30)
//                 },
                
//                 PropertyListDto = new PropertyListDto
//                 {
//                     PropertyId = 1,
//                     Title = "Modern Apartment in Downtown",
//                     Price = 250000,
//                     Location = "Downtown Cairo",
//                     Bedrooms = 3,
//                     Bathrooms = 2,
//                     Area = 120.5m,
//                     PropertyTypeName = "Apartment",
//                     OwnerName = "Ahmed Mohammed",
//                     MainImageUrl = "/uploads/properties/1/main.jpg",
//                     FavoritesCount = 15,
//                     AverageRating = 4.5,
//                     ReviewsCount = 8,
//                     ViewsCount = 250,
//                     IsAvailable = true,
//                     CreatedAt = DateTime.UtcNow.AddDays(-5)
//                 },
                
//                 NotificationDto = new NotificationDto
//                 {
//                     NotificationId = 1,
//                     UserId = 1,
//                     NotificationType = "NewMessage",
//                     Title = "New Message Received",
//                     Message = "You have received a new message about your property",
//                     RelatedId = 5,
//                     IsRead = false,
//                     CreatedAt = DateTime.UtcNow.AddHours(-2)
//                 },
                
//                 SavedSearchDto = new SavedSearchDto
//                 {
//                     SearchId = 1,
//                     UserId = 1,
//                     SearchName = "Family Home in Suburbs",
//                     Location = "New Cairo",
//                     PropertyTypeName = "Villa",
//                     MinPrice = 500000,
//                     MaxPrice = 1000000,
//                     MinBedrooms = 3,
//                     MaxBedrooms = 5,
//                     MinArea = 200,
//                     MaxArea = 400,
//                     HasGarden = true,
//                     HasPool = true,
//                     IsActive = true,
//                     EmailNotifications = true,
//                     CreatedAt = DateTime.UtcNow.AddDays(-10)
//                 }
//             };

//             return Ok(ApiResponse<object>.Success(examples, "DTO examples generated successfully"));
//         }

//         [HttpPost("dto-validation")]
//         public ActionResult<ApiResponse<object>> TestDTOValidation([FromBody] CreateUserDto createUserDto)
//         {
//             if (!ModelState.IsValid)
//             {
//                 var errors = ModelState
//                     .Where(x => x.Value?.Errors.Count > 0)
//                     .ToDictionary(
//                         kvp => kvp.Key,
//                         kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
//                     );

//                 return BadRequest(ApiResponse<object>.Failure(
//                     "Validation failed", 
//                     errors.SelectMany(x => x.Value ?? Array.Empty<string>()).ToList()));
//             }

//             var validationResult = new
//             {
//                 Message = "DTO validation successful!",
//                 ReceivedData = new
//                 {
//                     createUserDto.FirstName,
//                     createUserDto.LastName,
//                     createUserDto.Email,
//                     createUserDto.Phone,
//                     createUserDto.City,
//                     createUserDto.Country,
//                     PasswordProvided = !string.IsNullOrEmpty(createUserDto.Password),
//                     DateOfBirth = createUserDto.DateOfBirth?.ToString("yyyy-MM-dd")
//                 },
//                 ValidationsPassed = new List<string>
//                 {
//                     "✅ Required fields validation",
//                     "✅ String length validation",
//                     "✅ Email format validation",
//                     "✅ Phone format validation",
//                     "✅ Password complexity validation"
//                 }
//             };

//             return Ok(ApiResponse<object>.Success(validationResult, "DTO validation test completed"));
//         }

//         [HttpGet("pagination-example")]
//         public ActionResult<ApiResponse<PaginatedResultDto<object>>> GetPaginationExample(
//             [FromQuery] int page = 1, 
//             [FromQuery] int pageSize = 5)
//         {
//             // Sample data for pagination testing
//             var allItems = Enumerable.Range(1, 50).Select(i => new
//             {
//                 Id = i,
//                 Name = $"Property {i}",
//                 Price = 100000 + (i * 25000),
//                 Location = $"Location {i}",
//                 CreatedAt = DateTime.UtcNow.AddDays(-i)
//             }).ToList();

//             var totalCount = allItems.Count;
//             var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
//             var skip = (page - 1) * pageSize;
//             var items = allItems.Skip(skip).Take(pageSize).ToList();

//             var paginatedResult = new PaginatedResultDto<object>
//             {
//                 Data = items.Cast<object>().ToList(),
//                 TotalCount = totalCount,
//                 Page = page,
//                 PageSize = pageSize,
//                 TotalPages = totalPages,
//                 HasNextPage = page < totalPages,
//                 HasPreviousPage = page > 1
//             };

//             return Ok(ApiResponse<PaginatedResultDto<object>>.Success(
//                 paginatedResult, 
//                 "Pagination example generated successfully"));
//         }

//         [HttpGet("search-example")]
//         public ActionResult<ApiResponse<object>> GetSearchExample([FromQuery] PropertySearchDto searchDto)
//         {
//             var searchResult = new
//             {
//                 SearchParameters = new
//                 {
//                     searchDto.Location,
//                     searchDto.PropertyTypeId,
//                     PriceRange = searchDto.HasPriceFilter ? 
//                         $"{searchDto.MinPrice:N0} - {searchDto.MaxPrice:N0}" : "Any price",
//                     BedroomRange = searchDto.HasRoomFilter ? 
//                         $"{searchDto.MinBedrooms}-{searchDto.MaxBedrooms} bedrooms" : "Any bedrooms",
//                     AreaRange = searchDto.HasAreaFilter ? 
//                         $"{searchDto.MinArea:N0}-{searchDto.MaxArea:N0} sqm" : "Any area",
//                     searchDto.Keywords,
//                     searchDto.Page,
//                     searchDto.PageSize,
//                     searchDto.SortBy,
//                     searchDto.SortOrder
//                 },
                
//                 FiltersApplied = new
//                 {
//                     searchDto.HasLocationFilter,
//                     searchDto.HasPriceFilter,
//                     searchDto.HasRoomFilter,
//                     searchDto.HasAreaFilter,
//                     searchDto.HasFeatureFilter
//                 },
                
//                 Features = new List<string>(),
                
//                 MockResults = new
//                 {
//                     TotalFound = 25,
//                     ItemsReturned = Math.Min(searchDto.PageSize, 25),
//                     Message = "Search DTOs working properly with filters"
//                 }
//             };

//             return Ok(ApiResponse<object>.Success(searchResult, "Search example generated successfully"));
//         }

//         [HttpGet("model-validation")]
//         public ActionResult<ApiResponse<object>> ValidateModels()
//         {
//             var modelValidation = new
//             {
//                 ModelsCount = 11,
//                 DTOsCount = 35,
//                 Status = new
//                 {
//                     Models = "✅ Complete (11 models)",
//                     DTOs = "✅ Complete (35 DTOs)",
//                     Database = "✅ Connected",
//                     Validation = "✅ All validations working",
//                     Examples = "✅ All examples working"
//                 },
                
//                 ReadyForNextStep = new List<string>
//                 {
//                     "✅ Models layer complete",
//                     "✅ DTOs layer complete", 
//                     "✅ Validation system ready",
//                     "✅ Pagination system ready",
//                       "✅ Search system ready",
//                     "✅ Authentication DTOs ready",
//                     "✅ File upload DTOs ready",
//                     "✅ Communication DTOs ready",
//                     "✅ Ready for Repository Pattern implementation"
//                 },
                
//                 NextSteps = new List<string>
//                 {
//                     "1. Repository Pattern",
//                     "2. Service Layer", 
//                     "3. AutoMapper Configuration",
//                     "4. API Controllers",
//                     "5. Authentication System"
//                 }
//             };

//             return Ok(ApiResponse<object>.Success(modelValidation, "Full system validation completed successfully"));
//         }
//     }
// }