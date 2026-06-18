🚀 E-Commerce Web API (.NET 8 + ASP.NET Core + SQL Server + JWT)

A production-ready E-Commerce Backend API built using ASP.NET Core Web API, Entity Framework Core, SQL Server, and JWT Authentication. This project demonstrates modern backend development practices including Authentication, Authorization, Product Management, Category Management, Shopping Cart, Order Management, Search, Filtering, Sorting, and Pagination.

📌 Features
🔐 Authentication & Authorization
User Registration
User Login
JWT Token Generation
Role-Based Authorization (Admin / Customer)
Secure API Endpoints
📂 Category Management
Create Category
Update Category
Delete Category
Get All Categories
Get Category By Id
📦 Product Management
Create Product
Update Product
Delete Product
Get Product By Id
Get All Products
Category-Product Relationship
🔍 Search, Filter & Pagination
Product Search
Category Filtering
Price Sorting
Name Sorting
Pagination Support
🛒 Shopping Cart
Add To Cart
Update Quantity
Remove Product
Get My Cart
Calculate Cart Total
📋 Order Management
Checkout
Create Order
Order History
Order Details
Order Status Management
Stock Quantity Updates
🏗️ Tech Stack
Technology	Usage
ASP.NET Core Web API	Backend Development
Entity Framework Core	ORM
SQL Server	Database
JWT Authentication	Security
Swagger	API Documentation
LINQ	Query Operations
BCrypt.Net	Password Hashing
📂 Project Structure
ECommerce_WebAPI
│
├── Controllers
│   ├── AuthController
│   ├── CategoryController
│   ├── ProductController
│   ├── CartController
│   └── OrderController
│
├── Models
│   ├── User
│   ├── Category
│   ├── Product
│   ├── Cart
│   ├── CartItem
│   ├── Order
│   └── OrderItem
│
├── DTOs
│
├── Data
│   └── EcomDbContext
│
├── Services
│   └── JwtService
│
└── Migrations
🗄️ Database Design
User
 │
 ├── Cart
 │     │
 │     └── CartItems
 │              │
 │              └── Product
 │
 └── Orders
        │
        └── OrderItems
                 │
                 └── Product

Category
   │
   └── Products
⚙️ API Endpoints
Authentication
Method	Endpoint
POST	/api/Auth/Register
POST	/api/Auth/Login
Categories
Method	Endpoint
GET	/api/Category
GET	/api/Category/{id}
POST	/api/Category
PUT	/api/Category/{id}
DELETE	/api/Category/{id}
Products
Method	Endpoint
GET	/api/Product
GET	/api/Product/{id}
POST	/api/Product
PUT	/api/Product/{id}
DELETE	/api/Product/{id}
Shopping Cart
Method	Endpoint
POST	/api/Cart/AddToCart
GET	/api/Cart/MyCart
PUT	/api/Cart/UpdateQuantity
DELETE	/api/Cart/Remove/{productId}
Orders
Method	Endpoint
POST	/api/Order/Checkout
GET	/api/Order/MyOrders
GET	/api/Order/{id}
PUT	/api/Order/Status
🚀 Getting Started
Clone Repository
git clone https://github.com/yourusername/ecommerce-webapi.git
Navigate Project
cd ecommerce-webapi
Restore Packages
dotnet restore
Update Connection String
"ConnectionStrings": {
  "cs": "Server=.;Database=ECommerceDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
Apply Migrations
Add-Migration InitialCreate

Update-Database
Run Project
dotnet run
🔑 JWT Configuration
"Jwt": {
  "Key": "YourSuperSecretKeyMinimum32Characters",
  "Issuer": "ECommerceAPI",
  "Audience": "ECommerceUsers"
}
📸 Screenshots
Swagger UI
![Swagger Screenshot](images/swagger-home.png)
Authentication API
![Login API](images/login-api.png)
Product Management
![Products](images/products-api.png)
Shopping Cart
![Cart](images/cart-api.png)
Order Management
![Orders](images/orders-api.png)

Create an images folder in your repository and upload screenshots there.

🎯 Learning Outcomes
ASP.NET Core Web API
Entity Framework Core
SQL Server Integration
JWT Authentication & Authorization
RESTful API Development
LINQ Queries
CRUD Operations
Pagination & Filtering
Shopping Cart Implementation
Order Processing Workflow
👨‍💻 Author

Vickey Yadav

Aspiring .NET Full Stack Developer | ASP.NET Core | C# | SQL Server | Web API | React

Connect With Me
LinkedIn: [https://linkedin.com/in/your-linkedin](https://www.linkedin.com/in/vickey-yadav-1a08b324b/)
GitHub: [https://github.com/your-github](https://github.com/Vickey0088?tab=repositories)
⭐ Support

If you found this project useful, please consider giving it a ⭐ on GitHub. It helps others discover the project and motivates further development.
