using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace VoluntariosConectadosRD.Models.DTOs
{
    public class UserBasicDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
    }

    public enum MessageType
    {
        Text = 1,
        Image = 2,
        File = 3,
        System = 4
    }

    public enum MessageStatus
    {
        Sent = 1,
        Delivered = 2,
        Read = 3
    }

    public class MessageDto
    {
        public int Id { get; set; }
        public int ConversationId { get; set; }
        public int SenderId { get; set; }
        public string Content { get; set; } = string.Empty;
        public MessageType Type { get; set; } = MessageType.Text;
        public DateTime SentAt { get; set; }
        public MessageStatus Status { get; set; } = MessageStatus.Sent;
        public string? AttachmentUrl { get; set; }
        public string? AttachmentName { get; set; }
        public UserBasicDto Sender { get; set; } = new UserBasicDto();
        public UserBasicDto Recipient { get; set; } = new UserBasicDto();

        // Additional properties for UI
        public bool IsFromCurrentUser { get; set; }
        public bool IsRead { get; set; }
        public string TimeAgo { get; set; } = string.Empty;
    }

    public class ConversationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime LastMessageAt { get; set; }
        public string LastMessageContent { get; set; } = string.Empty;
        public bool HasUnreadMessages { get; set; }
        public int UnreadCount { get; set; }
        public List<MessageDto> Messages { get; set; } = new();
        public List<UserBasicDto> Participants { get; set; } = new();
        
        // For direct messages
        public bool IsDirectMessage { get; set; }
        public UserBasicDto OtherUser { get; set; } = new UserBasicDto();
    }

    public class CreateMessageDto
    {
        [Required]
        public int ConversationId { get; set; }
        
        [Required]
        [StringLength(5000)]
        public string Content { get; set; } = string.Empty;
        
        public MessageType Type { get; set; } = MessageType.Text;
        public string? AttachmentUrl { get; set; }
        public string? AttachmentName { get; set; }
    }

    public class CreateConversationDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public List<int> ParticipantIds { get; set; } = new();
        
        public bool IsDirectMessage { get; set; }
    }

    public class ConversationListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime LastActivity { get; set; }
        public string LastMessage { get; set; } = string.Empty;
        public int UnreadCount { get; set; }
        public bool IsDirectMessage { get; set; }
        public UserBasicDto OtherUser { get; set; } = new UserBasicDto();
        public string TimeAgo { get; set; } = string.Empty;
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

    public class ConversationStatsDto
    {
        public int TotalConversations { get; set; }
        public int UnreadConversations { get; set; }
        public int TotalUnreadMessages { get; set; }
        public DateTime? LastActivity { get; set; }
    }
}