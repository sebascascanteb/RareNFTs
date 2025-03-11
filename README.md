# RareNFTs Project Description
This project is a platform for managing and selling NFTs (Non-Fungible Tokens) developed as part of the ISW-711: Web Environment Programming 2 course at the Universidad Técnica Nacional. NFTs are unique digital assets stored on a blockchain, allowing traceability and authenticity of digital ownership. The goal of the project is to design a system that allows the complete cycle of NFT sales, manage clients and their crypto wallets, query NFTs, control ownership, and generate various reports and charts.

## Technologies Used
**Programming Language:** C# (.NET Core 8)

**Development Framework:** ASP.NET MVC

**Database:** SQL Server 2019

**Frontend:** Bootstrap for UI elements, Toastr and SweetAlert for notifications

**Architecture:** Clean Architecture, implementing SOLID principles and design patterns like MVC, Repository, Unit of Work, DTO, Factory, Dependency Injection

**Security:** User and role management (Administrator, Processes, Reports)

**REST API:** Exposing endpoints for NFT queries

**Error Handling:** Serilog for logging

**Validation and Error Management:** Client-side and server-side validations using AJAX and form validations

## Main Features
### NFT Sales Management

Selling NFTs with invoice sent via email in PDF format
Canceling sales and removing NFTs from the blockchain
Changing NFT ownership

### Data Maintenance:

Client and crypto wallet management
Payment card registration (MasterCard, Visa, American Express)
NFT asset management (code, name, author, value, image)
Country management (ID and description)

### Queries and Reports:

Query NFT owners by name
List of clients and sales exportable to PDF
Complete list of registered NFTs with details and images
Sales charts by date range

### Interoperability via REST API:

Query NFTs by name and owner data
List all available NFTs

## Technical Requirements
**Normalization:** Database normalized to 3NFBC

**Indexes:** Use of Clustered and Non-Clustered indexes

**Object-Oriented Paradigm:** Application of abstraction, encapsulation, modularity, and hierarchy principles

**Security:** Implementation of user roles and access validations

**User Interface:** Use of Bootstrap for UI elements and AJAX form validations

This project represents a comprehensive implementation of an NFT management system, ensuring traceability and authenticity of digital assets while providing an intuitive and secure user experience.
 
