using Microsoft.AspNetCore.Mvc;
using VoluntariosConectadosRD.Models;
using VoluntariosConectadosRD.Services;
using VoluntariosConectadosRD.Models.DTOs;
using VoluntariadoConectadoRD.Models.DTOs;

namespace VoluntariosConectadosRD.Controllers
{
    public class MessagesController : Controller
    {
        private readonly ILogger<MessagesController> _logger;
        private readonly IBaseApiService _baseApiService;

        public MessagesController(ILogger<MessagesController> logger, IBaseApiService baseApiService)
        {
            _logger = logger;
            _baseApiService = baseApiService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{conversationId}")]
        public IActionResult Conversation(string conversationId)
        {
            ViewBag.ConversationId = conversationId;
            return View();
        }

        [HttpGet("new/{recipientId:int}")]
        public IActionResult New(int recipientId)
        {
            ViewBag.RecipientId = recipientId;
            return View();
        }

        // API endpoints for frontend

        [HttpGet("api/conversations")]
        public async Task<IActionResult> GetConversations(int page = 1, int pageSize = 20)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWTToken");
                if (string.IsNullOrEmpty(token))
                {
                    return Json(new { success = false, message = "Sesión expirada" });
                }

                _baseApiService.SetAuthToken(token);
                var response = await _baseApiService.GetAsync<ApiResponseDto<ConversationListDto>>(
                    $"api/Message/conversations?page={page}&pageSize={pageSize}");
                
                return Json(new { 
                    success = response?.Success ?? false, 
                    data = response?.Data,
                    message = response?.Message 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting conversations");
                return Json(new { 
                    success = false, 
                    message = "Error de conexión al obtener conversaciones" 
                });
            }
        }

        [HttpGet("api/conversation/{conversationId}/messages")]
        public async Task<IActionResult> GetConversationMessages(string conversationId, int page = 1, int pageSize = 50)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWTToken");
                if (string.IsNullOrEmpty(token))
                {
                    return Json(new { success = false, message = "Sesión expirada" });
                }

                _baseApiService.SetAuthToken(token);
                var response = await _baseApiService.GetAsync<ApiResponseDto<ConversationMessagesDto>>(
                    $"api/Message/conversation/{conversationId}/messages?page={page}&pageSize={pageSize}");
                
                return Json(new { 
                    success = response?.Success ?? false, 
                    data = response?.Data,
                    message = response?.Message 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting conversation messages");
                return Json(new { 
                    success = false, 
                    message = "Error de conexión al obtener mensajes" 
                });
            }
        }

        [HttpPost("api/send")]
        public async Task<IActionResult> SendMessage([FromForm] SendMessageDto messageDto)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWTToken");
                if (string.IsNullOrEmpty(token))
                {
                    return Json(new { success = false, message = "Sesión expirada" });
                }

                // Create multipart form data
                using var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(messageDto.RecipientId.ToString()), "RecipientId");
                formData.Add(new StringContent(messageDto.Content), "Content");
                formData.Add(new StringContent(((int)messageDto.Type).ToString()), "Type");

                if (messageDto.ReplyToMessageId.HasValue)
                {
                    formData.Add(new StringContent(messageDto.ReplyToMessageId.Value.ToString()), "ReplyToMessageId");
                }

                if (messageDto.Attachment != null)
                {
                    var fileContent = new StreamContent(messageDto.Attachment.OpenReadStream());
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(messageDto.Attachment.ContentType);
                    formData.Add(fileContent, "Attachment", messageDto.Attachment.FileName);
                }

                var response = await _baseApiService.PostFormDataAsync<ApiResponseDto<MessageDto>>("api/Message/send", formData, token);
                
                return Json(new { 
                    success = response?.Success ?? false, 
                    data = response?.Data,
                    message = response?.Message 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message");
                return Json(new { 
                    success = false, 
                    message = "Error de conexión al enviar mensaje" 
                });
            }
        }

        [HttpPost("api/conversation/start")]
        public async Task<IActionResult> StartConversation([FromBody] StartConversationDto startDto)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWTToken");
                if (string.IsNullOrEmpty(token))
                {
                    return Json(new { success = false, message = "Sesión expirada" });
                }

                _baseApiService.SetAuthToken(token);
                var response = await _baseApiService.PostAsync<ApiResponseDto<ConversationDto>>("api/Message/conversation/start", startDto);
                
                return Json(new { 
                    success = response?.Success ?? false, 
                    data = response?.Data,
                    message = response?.Message 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting conversation");
                return Json(new { 
                    success = false, 
                    message = "Error de conexión al iniciar conversación" 
                });
            }
        }

        [HttpPut("api/conversation/{conversationId}/read")]
        public async Task<IActionResult> MarkMessagesAsRead(string conversationId)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWTToken");
                if (string.IsNullOrEmpty(token))
                {
                    return Json(new { success = false, message = "Sesión expirada" });
                }

                _baseApiService.SetAuthToken(token);
                var response = await _baseApiService.PutAsync<ApiResponse<object>>($"api/Message/conversation/{conversationId}/read", null);
                
                return Json(new { 
                    success = response?.Success ?? false, 
                    data = response?.Data,
                    message = response?.Message 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking messages as read");
                return Json(new { 
                    success = false, 
                    message = "Error de conexión al marcar mensajes como leídos" 
                });
            }
        }

        [HttpPut("api/message/{messageId:int}")]
        public async Task<IActionResult> EditMessage(int messageId, [FromBody] EditMessageDto editDto)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWTToken");
                if (string.IsNullOrEmpty(token))
                {
                    return Json(new { success = false, message = "Sesión expirada" });
                }

                _baseApiService.SetAuthToken(token);
                var response = await _baseApiService.PutAsync<ApiResponseDto<MessageDto>>($"api/Message/{messageId}", editDto);
                
                return Json(new { 
                    success = response?.Success ?? false, 
                    data = response?.Data,
                    message = response?.Message 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing message");
                return Json(new { 
                    success = false, 
                    message = "Error de conexión al editar mensaje" 
                });
            }
        }

        [HttpDelete("api/message/{messageId:int}")]
        public async Task<IActionResult> DeleteMessage(int messageId)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWTToken");
                if (string.IsNullOrEmpty(token))
                {
                    return Json(new { success = false, message = "Sesión expirada" });
                }

                _baseApiService.SetAuthToken(token);
                var response = await _baseApiService.DeleteAsync($"api/Message/{messageId}");
                
                return Json(new { 
                    success = response, 
                    message = response ? "Mensaje eliminado" : "Error al eliminar mensaje"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting message");
                return Json(new { 
                    success = false, 
                    message = "Error de conexión al eliminar mensaje" 
                });
            }
        }

        [HttpGet("api/stats")]
        public async Task<IActionResult> GetConversationStats()
        {
            try
            {
                var token = HttpContext.Session.GetString("JWTToken");
                if (string.IsNullOrEmpty(token))
                {
                    return Json(new { success = false, message = "Sesión expirada" });
                }

                _baseApiService.SetAuthToken(token);
                var response = await _baseApiService.GetAsync<ApiResponseDto<ConversationStatsDto>>("api/Message/stats");
                
                return Json(new { 
                    success = response?.Success ?? false, 
                    data = response?.Data,
                    message = response?.Message 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting conversation stats");
                return Json(new { 
                    success = false, 
                    message = "Error de conexión al obtener estadísticas" 
                });
            }
        }
    }
}