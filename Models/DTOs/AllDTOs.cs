using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace VoluntariosConectadosRD.Models.DTOs
{
    // Common API Response wrapper
    public class ApiResponseDto<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
    }

    // Pagination
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}

// Backend namespace compatibility
namespace VoluntariadoConectadoRD.Models.DTOs
{
    // AuthDTOs.cs
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }

    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public string UserType { get; set; } = "Voluntario"; // "Voluntario" or "ONG"
    }

    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }

    public class ResetPasswordDto
    {
        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class ChangePasswordDto
    {
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }

    public class UserBasicDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
    }

    // MessageDTOs.cs
    public enum MessageType
    {
        Text = 1,
        Image = 2,
        File = 3,
        System = 4,
        ApplicationUpdate = 5
    }

    public class MessageDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public string Content { get; set; } = string.Empty;
        public MessageType Type { get; set; }
        public bool IsRead { get; set; }
        public DateTime SentAt { get; set; }
        public DateTime? ReadAt { get; set; }
        public DateTime? EditedAt { get; set; }
        public int? ReplyToMessageId { get; set; }
        public string? AttachmentUrl { get; set; }
        public string? AttachmentFileName { get; set; }
        public string? AttachmentMimeType { get; set; }
        public long? AttachmentSize { get; set; }
        public string ConversationId { get; set; } = string.Empty;
        public UserBasicDto Sender { get; set; } = new UserBasicDto();
        public UserBasicDto Recipient { get; set; } = new UserBasicDto();
        public MessageDto? ReplyToMessage { get; set; }
        public string TimeAgo { get; set; } = string.Empty;
        public bool IsFromCurrentUser { get; set; }
        public string FormattedContent { get; set; } = string.Empty;
    }

    public class SendMessageDto
    {
        [Required]
        public int RecipientId { get; set; }

        [Required]
        [StringLength(2000, MinimumLength = 1)]
        public string Content { get; set; } = string.Empty;

        public MessageType Type { get; set; } = MessageType.Text;

        public int? ReplyToMessageId { get; set; }

        // For file attachments
        public IFormFile? Attachment { get; set; }
    }

    public class ConversationDto
    {
        public string Id { get; set; } = string.Empty;
        public UserBasicDto OtherUser { get; set; } = new UserBasicDto();
        public MessageDto? LastMessage { get; set; }
        public DateTime LastMessageAt { get; set; }
        public bool HasUnread { get; set; }
        public int UnreadCount { get; set; }
        public DateTime? LastSeen { get; set; }
        public bool IsOnline { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ConversationListDto
    {
        public List<ConversationDto> Conversations { get; set; } = new List<ConversationDto>();
        public int TotalUnread { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNextPage => Page < TotalPages;
        public bool HasPreviousPage => Page > 1;
    }

    public class ConversationMessagesDto
    {
        public string ConversationId { get; set; } = string.Empty;
        public UserBasicDto OtherUser { get; set; } = new UserBasicDto();
        public List<MessageDto> Messages { get; set; } = new List<MessageDto>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNextPage => Page < TotalPages;
        public bool HasPreviousPage => Page > 1;
        public bool IsOtherUserOnline { get; set; }
        public DateTime? OtherUserLastSeen { get; set; }
        public bool IsTyping { get; set; }
    }

    public class StartConversationDto
    {
        [Required]
        public int RecipientId { get; set; }

        [Required]
        [StringLength(2000, MinimumLength = 1)]
        public string InitialMessage { get; set; } = string.Empty;

        public string? Subject { get; set; }
    }

    public class EditMessageDto
    {
        [Required]
        [StringLength(2000, MinimumLength = 1)]
        public string Content { get; set; } = string.Empty;
    }

    public class ConversationStatsDto
    {
        public int TotalConversations { get; set; }
        public int UnreadConversations { get; set; }
        public int TotalUnreadMessages { get; set; }
        public DateTime? LastActivity { get; set; }
    }

    public class TypingIndicatorDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string ConversationId { get; set; } = string.Empty;
        public bool IsTyping { get; set; }
    }

    // OpportunityDTOs.cs
    public class CreateOpportunityDto
    {
        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "El título debe tener entre 5 y 100 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(2000, MinimumLength = 20, ErrorMessage = "La descripción debe tener entre 20 y 2000 caracteres.")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        public DateTime? FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
        public DateTime? FechaFin { get; set; }

        [Required(ErrorMessage = "La ubicación es obligatoria.")]
        [StringLength(200, ErrorMessage = "La ubicación no puede exceder los 200 caracteres.")]
        public string Ubicacion { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "La duración debe ser de al menos 1 hora.")]
        public int DuracionHoras { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Se requiere al menos 1 voluntario.")]
        public int VoluntariosRequeridos { get; set; }

        public List<string> Requisitos { get; set; } = new List<string>();
        public List<string> Tareas { get; set; } = new List<string>();

        [Required(ErrorMessage = "Debe seleccionar al menos una categoría.")]
        public string Categoria { get; set; } = string.Empty;
    }

    public class UpdateOpportunityDto : CreateOpportunityDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "El título debe tener entre 5 y 100 caracteres.")]
        public new string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(2000, MinimumLength = 20, ErrorMessage = "La descripción debe tener entre 20 y 2000 caracteres.")]
        public new string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        public new DateTime? FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
        public new DateTime? FechaFin { get; set; }

        [Required(ErrorMessage = "La ubicación es obligatoria.")]
        [StringLength(200, ErrorMessage = "La ubicación no puede exceder los 200 caracteres.")]
        public new string Ubicacion { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "La duración debe ser de al menos 1 hora.")]
        public new int DuracionHoras { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Se requiere al menos 1 voluntario.")]
        public new int VoluntariosRequeridos { get; set; }
    }

    public class OpportunityListDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public string Ubicacion { get; set; } = string.Empty;
        public int VoluntariosRequeridos { get; set; }
        public int VoluntariosInscritos { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public string ONG { get; set; } = string.Empty;
        public string? ONGLogoUrl { get; set; }
    }

    public class OpportunityDetailDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Ubicacion { get; set; } = string.Empty;
        public int DuracionHoras { get; set; }
        public int VoluntariosRequeridos { get; set; }
        public int VoluntariosInscritos { get; set; }
        public List<string> Requisitos { get; set; } = new List<string>();
        public List<string> Tareas { get; set; } = new List<string>();
        public string Categoria { get; set; } = string.Empty;
        public OrganizationInfoDto ONG { get; set; } = new OrganizationInfoDto();
        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }
        public List<VolunteerInfoDto> Voluntarios { get; set; } = new List<VolunteerInfoDto>();
    }

    // SearchDTOs.cs
    public class OpportunitySearchDto
    {
        public string? SearchTerm { get; set; }
        public string? Category { get; set; }
        public string? Location { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? MinDuration { get; set; }
        public int? MaxDuration { get; set; }
        public List<string>? Skills { get; set; }
        public string? SortBy { get; set; } = "Date"; // Date, Title, Duration, Volunteers
        public string? SortOrder { get; set; } = "Desc"; // Asc, Desc
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class VolunteerSearchDto
    {
        public string? SearchTerm { get; set; }
        public List<string>? Skills { get; set; }
        public int? MinExperience { get; set; }
        public int? MaxExperience { get; set; }
        public string? Location { get; set; }
        public string? Availability { get; set; }
        public double? MinRating { get; set; }
        public bool? IsActive { get; set; }
        public string? SortBy { get; set; } = "Rating"; // Name, Rating, Experience, JoinDate
        public string? SortOrder { get; set; } = "Desc";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class OrganizationSearchDto
    {
        public string? SearchTerm { get; set; }
        public string? Location { get; set; }
        public string? Category { get; set; }
        public bool? IsVerified { get; set; }
        public string? SortBy { get; set; } = "Name"; // Name, Rating, CreatedDate
        public string? SortOrder { get; set; } = "Asc";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class SearchResultDto<T>
    {
        public List<T> Results { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNextPage => Page < TotalPages;
        public bool HasPreviousPage => Page > 1;
        public SearchFilters? AppliedFilters { get; set; }
    }

    public class SearchFilters
    {
        public List<FilterOption> Categories { get; set; } = new List<FilterOption>();
        public List<FilterOption> Locations { get; set; } = new List<FilterOption>();
        public List<FilterOption> Skills { get; set; } = new List<FilterOption>();
        public List<FilterOption> Organizations { get; set; } = new List<FilterOption>();
        public DateRange? DateRange { get; set; }
        public NumberRange? DurationRange { get; set; }
        public NumberRange? ExperienceRange { get; set; }
    }

    public class FilterOption
    {
        public string Value { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public int Count { get; set; }
        public bool IsSelected { get; set; }
    }

    public class DateRange
    {
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
    }

    public class NumberRange
    {
        public int? Min { get; set; }
        public int? Max { get; set; }
    }

    public class QuickSearchDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Query { get; set; } = string.Empty;
        
        public string? Type { get; set; } = "all"; // all, opportunities, volunteers, organizations
        public int Limit { get; set; } = 5;
    }

    public class QuickSearchResultDto
    {
        public List<SearchSuggestion> Opportunities { get; set; } = new List<SearchSuggestion>();
        public List<SearchSuggestion> Volunteers { get; set; } = new List<SearchSuggestion>();
        public List<SearchSuggestion> Organizations { get; set; } = new List<SearchSuggestion>();
        public int TotalResults { get; set; }
    }

    public class SearchSuggestion
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Subtitle { get; set; }
        public string? ImageUrl { get; set; }
        public string Type { get; set; } = string.Empty; // opportunity, volunteer, organization
        public string Url { get; set; } = string.Empty;
        public double? Rating { get; set; }
        public string? Location { get; set; }
        public DateTime? Date { get; set; }
    }

    // TransparencyDTOs.cs
    public class FinancialReportDto
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string ReportName { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Quarter { get; set; } = string.Empty;
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public string ReportUrl { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
    }

    public class ProjectReportDto
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Budget { get; set; }
        public string ReportUrl { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
    }

    public class ImpactMetricDto
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string MetricName { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    // VolunteerDTOs.cs
    public class VolunteerInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public double Rating { get; set; }
        public int CompletedOpportunities { get; set; }
        public DateTime MemberSince { get; set; }
    }

    public class OrganizationInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? LogoUrl { get; set; }
        public string? Mission { get; set; }
        public string? Website { get; set; }
        public double Rating { get; set; }
        public int OpportunitiesPosted { get; set; }
        public bool IsVerified { get; set; }
    }

    // Backend compatibility DTOs
    public class LoginResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public UserInfoDto User { get; set; } = new();
    }

    public class UserInfoDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public int Rol { get; set; }
        public string RolNombre { get; set; } = string.Empty;
        public int Estatus { get; set; }
        public string? ProfileImageUrl { get; set; }
        public bool PerfilCompleto { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class UserProfileDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? Biografia { get; set; }
        public string? Habilidades { get; set; }
        public int? ExperienciaAnios { get; set; }
        public string? Disponibilidad { get; set; }
        public int HorasVoluntariado { get; set; }
        public decimal CalificacionPromedio { get; set; }
        public int TotalResenas { get; set; }
        public bool PerfilCompleto { get; set; }
    }

    public class EnhancedUserProfileDto : UserProfileDto
    {
        public List<string> Skills { get; set; } = new();
        public string? About { get; set; }
        public string? Location { get; set; }
        public bool IsAvailable { get; set; } = true;
    }

    public class OrganizationProfileDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? SitioWeb { get; set; }
        public string? LogoUrl { get; set; }
        public string? TipoOrganizacion { get; set; }
        public string? Mision { get; set; }
        public string? Vision { get; set; }
        public DateTime? FechaFundacion { get; set; }
        public string? NumeroRegistro { get; set; }
        public bool Verificada { get; set; }
        public int CalificacionPromedio { get; set; }
        public int TotalOportunidades { get; set; }
    }

    public class AdminStatsDto
    {
        public int TotalUsers { get; set; }
        public int TotalOrganizations { get; set; }
        public int TotalOpportunities { get; set; }
        public int TotalApplications { get; set; }
        public int ActiveUsers { get; set; }
        public int NewUsersThisMonth { get; set; }
        public decimal AverageRating { get; set; }
        public List<MonthlyStatsDto> MonthlyStats { get; set; } = new();
    }

    public class MonthlyStatsDto
    {
        public string Month { get; set; } = string.Empty;
        public int Users { get; set; }
        public int Organizations { get; set; }
        public int Opportunities { get; set; }
        public int Applications { get; set; }
    }

    public class AdminVolunteerDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public int Estatus { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int HorasVoluntariado { get; set; }
        public decimal CalificacionPromedio { get; set; }
        public string? ProfileImageUrl { get; set; }
        public bool PerfilCompleto { get; set; }
    }

    public class VolunteerApplicationDetailDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int OpportunityId { get; set; }
        public string? Mensaje { get; set; }
        public int Estatus { get; set; }
        public DateTime FechaAplicacion { get; set; }
        public DateTime? FechaRespuesta { get; set; }
        public string? NotasOrganizacion { get; set; }
        public string VolunteerName { get; set; } = string.Empty;
        public string OpportunityTitle { get; set; } = string.Empty;
    }

    // Transparency DTOs
    public class TransparencyFiltersDto
    {
        public bool? SoloVerificadas { get; set; }
        public string? TipoOrganizacion { get; set; }
        public int? Año { get; set; }
        public int? Trimestre { get; set; }
    }

    public class OrganizationTransparencyDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? LogoUrl { get; set; }
        public string? TipoOrganizacion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Verificada { get; set; }
        public decimal TotalIngresosPeriodo { get; set; }
        public decimal TotalGastosPeriodo { get; set; }
        public decimal BalanceActual { get; set; }
        public int TotalReportes { get; set; }
        public DateTime? UltimoReporte { get; set; }
        public List<FinancialReportSummaryDto> ReportesRecientes { get; set; } = new();
    }

    public class FinancialReportSummaryDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int Año { get; set; }
        public int Trimestre { get; set; }
        public decimal TotalIngresos { get; set; }
        public decimal TotalGastos { get; set; }
        public decimal Balance { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool EsPublico { get; set; }
    }

    // Additional missing DTOs and Enums
    public enum UserStatus
    {
        Activo = 1,
        Inactivo = 2,
        Suspendido = 3,
        PendienteVerificacion = 4
    }

    public class AdminEditUserDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public UserStatus Estatus { get; set; }
        public string? Ubicacion { get; set; }
    }

    public class AdminOrganizationDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? TipoOrganizacion { get; set; }
        public bool Verificada { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int TotalOportunidades { get; set; }
        public UserStatus Estatus { get; set; }
    }

    public class AdminEditOrganizationDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? SitioWeb { get; set; }
        public string? TipoOrganizacion { get; set; }
        public bool Verificada { get; set; }
        public UserStatus Estatus { get; set; }
    }

    public class PlatformStatsDto
    {
        public int TotalUsers { get; set; }
        public int TotalOrganizations { get; set; }
        public int TotalOpportunities { get; set; }
        public int TotalApplications { get; set; }
        public int ActiveUsers { get; set; }
        public int NewUsersThisMonth { get; set; }
        public int PendingApplications { get; set; }
        public decimal TotalVolunteerHours { get; set; }
    }

    public class DashboardStatsDto
    {
        public int TotalApplications { get; set; }
        public int PendingApplications { get; set; }
        public int AcceptedApplications { get; set; }
        public int CompletedOpportunities { get; set; }
        public int TotalVolunteerHours { get; set; }
        public decimal AverageRating { get; set; }
        public int BadgesEarned { get; set; }
        public List<MonthlyActivityDto> MonthlyActivity { get; set; } = new();
    }

    public class MonthlyActivityDto
    {
        public string Month { get; set; } = string.Empty;
        public int Applications { get; set; }
        public int CompletedOpportunities { get; set; }
        public int VolunteerHours { get; set; }
    }

    public class VolunteerStatsDto
    {
        public int TotalApplications { get; set; }
        public int CompletedOpportunities { get; set; }
        public int TotalVolunteerHours { get; set; }
        public decimal AverageRating { get; set; }
        public int BadgesEarned { get; set; }
        public DateTime? LastActivity { get; set; }
        public List<MonthlyActivityDto> MonthlyActivity { get; set; } = new();
    }

    public class RecentActivityDto
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string? Url { get; set; }
    }

    public class OrganizationStatsDto
    {
        public int TotalOpportunities { get; set; }
        public int ActiveOpportunities { get; set; }
        public int TotalApplications { get; set; }
        public int AcceptedVolunteers { get; set; }
        public int CompletedOpportunities { get; set; }
        public decimal AverageRating { get; set; }
        public List<MonthlyStatsDto> MonthlyStats { get; set; } = new();
    }

    public class OrganizationEventDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public string Ubicacion { get; set; } = string.Empty;
        public int VoluntariosInscritos { get; set; }
        public int VoluntariosRequeridos { get; set; }
        public string Estatus { get; set; } = string.Empty;
    }

    public class UserEventDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public string Ubicacion { get; set; } = string.Empty;
        public string OrganizacionNombre { get; set; } = string.Empty;
        public string EstadoAplicacion { get; set; } = string.Empty;
    }

    public class UserDashboardDto
    {
        public UserProfileDto Profile { get; set; } = new();
        public DashboardStatsDto Stats { get; set; } = new();
        public List<UserEventDto> UpcomingEvents { get; set; } = new();
        public List<RecentActivityDto> RecentActivity { get; set; } = new();
    }

    public class OrganizationDashboardDto
    {
        public OrganizationProfileDto Profile { get; set; } = new();
        public OrganizationStatsDto Stats { get; set; } = new();
        public List<OrganizationEventDto> UpcomingEvents { get; set; } = new();
        public List<RecentActivityDto> RecentActivity { get; set; } = new();
    }

    // Complete transparency DTOs from backend
    public class OrganizationFinancialDetailsDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? SitioWeb { get; set; }
        public string? LogoUrl { get; set; }
        public string? TipoOrganizacion { get; set; }
        public string? Mision { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Verificada { get; set; }
        
        public FinancialSummaryDto ResumenFinanciero { get; set; } = new();
        public List<FinancialReportDetailDto> ReportesFinancieros { get; set; } = new();
        
        public ChartDataDto GastosCategoria { get; set; } = new();
        public ChartDataDto IngresosTipo { get; set; } = new();
        public ChartDataDto TendenciaTrimestral { get; set; } = new();
    }

    public class FinancialReportDetailDto
    {
        public int Id { get; set; }
        public int OrganizacionId { get; set; }
        public string OrganizacionNombre { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public int Año { get; set; }
        public int Trimestre { get; set; }
        public decimal TotalIngresos { get; set; }
        public decimal TotalGastos { get; set; }
        public decimal Balance { get; set; }
        public string? Resumen { get; set; }
        public string? DocumentoUrl { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        
        public List<ExpenseDto> Gastos { get; set; } = new();
        public List<DonationDto> Donaciones { get; set; } = new();
        
        public decimal TotalGastosOperativos => Gastos.Where(g => g.Categoria == "Operativo").Sum(g => g.Monto);
        public decimal TotalGastosPrograma => Gastos.Where(g => g.Categoria == "Programa").Sum(g => g.Monto);
        public decimal TotalGastosAdministrativos => Gastos.Where(g => g.Categoria == "Administrativo").Sum(g => g.Monto);
        public decimal TotalDonacionesMonetarias => Donaciones.Where(d => d.Tipo == "Monetaria").Sum(d => d.Monto);
        public decimal TotalDonacionesEspecie => Donaciones.Where(d => d.Tipo == "Especie").Sum(d => d.Monto);
    }

    public class FinancialSummaryDto
    {
        public decimal TotalIngresosHistorico { get; set; }
        public decimal TotalGastosHistorico { get; set; }
        public decimal BalanceGeneral { get; set; }
        public decimal PromedioIngresosTrimestral { get; set; }
        public decimal PromedioGastosTrimestral { get; set; }
        public int TotalReportes { get; set; }
        public int TotalDonantes { get; set; }
        public DateTime? PrimerReporte { get; set; }
        public DateTime? UltimoReporte { get; set; }
    }

    public class ChartDataDto
    {
        public List<string> Labels { get; set; } = new();
        public List<decimal> Values { get; set; } = new();
        public string? Title { get; set; }
    }

    public class ExpenseDto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string? Justificacion { get; set; }
        public string? DocumentoUrl { get; set; }
    }

    public class DonationDto
    {
        public int Id { get; set; }
        public string Donante { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string? Proposito { get; set; }
        public bool EsRecurrente { get; set; }
    }
}
