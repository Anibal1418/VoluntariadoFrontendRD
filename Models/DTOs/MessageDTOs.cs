using System.ComponentModel.DataAnnotations;

namespace VoluntariosConectadosRD.Models.DTOs
{
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
        
        // API Response wrapper properties
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public MessageDto? Data { get; set; }
        public List<string>? Errors { get; set; }
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
        
        // API Response wrapper properties
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public ConversationDto? Data { get; set; }
        public List<string>? Errors { get; set; }
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
        
        // API Response wrapper properties
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public ConversationListDto? Data { get; set; }
        public List<string>? Errors { get; set; }
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
        
        // API Response wrapper properties
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public ConversationMessagesDto? Data { get; set; }
        public List<string>? Errors { get; set; }
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
        
        // API Response wrapper properties
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public ConversationStatsDto? Data { get; set; }
        public List<string>? Errors { get; set; }
    }

    public class TypingIndicatorDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string ConversationId { get; set; } = string.Empty;
        public bool IsTyping { get; set; }
    }
}