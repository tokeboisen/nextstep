# Non-Functional Requirements

## Authentication & Authorization

### NFR-AUTH-001: Single User Authentication
- Applikationen har én fast bruger
- Login credentials konfigureres via environment variables:
  - `Auth__Email`
  - `Auth__Password`
- Simple username/password authentication
- JWT tokens for session management

---

## Technology Stack

### Frontend
- **Framework**: React
- **Language**: TypeScript

### Backend
- **Framework**: .NET (ASP.NET Core)
- **Language**: C#
- **Architecture**:
  - Domain Driven Design
  - CQRS pattern

### Database
- **Engine**: MySQL
- **ORM**: Entity Framework Core

### Deployment
- **Local Development**: Docker Desktop (single container med frontend + backend)
- **Production**: Simply.com
  - Web Deploy
  - MySQL database hosted på Simply.com

---

## Testing

### NFR-TEST-001: Unit Tests
- Alle domain entities skal have unit tests
- Alle command/query handlers skal have unit tests
- Coverage mål: Kritisk business logic

### NFR-TEST-002: Integration Tests
- API endpoints skal have integration tests
- Database operationer skal testes med in-memory eller test database

### Test Frameworks
- **Backend**: xUnit, FluentAssertions, Moq
- **Frontend**: Jest, React Testing Library

---

## Deployment Architecture

### Single Container Deployment
- Frontend og backend deployes som én enhed
- Backend serverer statisk frontend build
- Én Docker container til lokal udvikling

### Local Development
```
Docker Desktop
├── NextStep Container
│   ├── ASP.NET Core Backend (port 8080)
│   │   └── Serves React static files
│   └── React Frontend (built, static)
└── MySQL Container (port 3306)
```

### Production (Simply.com)
- Web Deploy til Simply.com hosting
- MySQL database på Simply.com
- Reference: Simply.com dokumentation for Web Deploy

---

## Code Quality

### NFR-CODE-001: Code Style
- C#: Standard .NET conventions
- TypeScript: ESLint + Prettier
- Consistent naming conventions

### NFR-CODE-002: API Design
- RESTful endpoints
- Consistent error handling
- API versioning (når relevant)

---

## UI and UX

### NFR-UX-001: Design System
- Google Material Design (MUI - Material UI for React)
- Consistent typography, spacing, and color palette
- Responsive design for desktop and mobile

### NFR-UX-002: Login Page Layout
- Split-screen layout
- Billede/illustration fylder venstre halvdel af skærmen
- Login form i højre side med centreret indhold
- Clean, minimalistisk design

### NFR-UX-003: Profile Page Layout
- Sidebar navigation i venstre side
- Profile sektioner organiseret vertikalt i sidebar
- Hovedindhold vises i center/højre område
- Logisk gruppering af relaterede data

### NFR-UX-004: Component Guidelines
- Brug Material UI komponenter konsekvent
- Floating labels på input felter
- Outlined input style
- Contained buttons til primære handlinger
- Text buttons til sekundære handlinger
- Cards med elevation til sektioner
