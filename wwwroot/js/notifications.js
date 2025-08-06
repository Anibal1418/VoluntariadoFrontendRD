class NotificationSystem {
    constructor() {
        this.notifications = [];
        this.currentFilter = 'all';
        this.init();
    }

    init() {
        this.loadMockData();
        this.setupEventListeners();
        this.updateNotificationCount();
        this.renderNotifications();
    }

    loadMockData() {
        this.notifications = [
            {
                id: 1,
                title: 'Nuevo evento de voluntariado',
                description: 'Se ha creado un nuevo evento de reforestación en el Parque Mirador Sur. ¡Únete y ayuda a hacer la diferencia!',
                timestamp: new Date(Date.now() - 2 * 60 * 60 * 1000), // 2 horas atrás
                read: false,
                type: 'event'
            },
            {
                id: 2,
                title: 'Recordatorio de sesión',
                description: 'Tu sesión de voluntariado en el Centro Comunitario de Los Mina está programada para mañana a las 9:00 AM.',
                timestamp: new Date(Date.now() - 4 * 60 * 60 * 1000), // 4 horas atrás
                read: false,
                type: 'reminder'
            },
            {
                id: 3,
                title: 'Actualización de perfil',
                description: 'Tu perfil ha sido actualizado exitosamente. Los nuevos voluntarios podrán ver tus habilidades actualizadas.',
                timestamp: new Date(Date.now() - 6 * 60 * 60 * 1000), // 6 horas atrás
                read: true,
                type: 'profile'
            },
            {
                id: 4,
                title: 'Nueva oportunidad disponible',
                description: 'Hay una nueva oportunidad de voluntariado en el Hospital Infantil Dr. Robert Reid Cabral. Revisa los detalles.',
                timestamp: new Date(Date.now() - 8 * 60 * 60 * 1000), // 8 horas atrás
                read: false,
                type: 'opportunity'
            },
            {
                id: 5,
                title: 'Mensaje de la ONG',
                description: 'La ONG "Ayuda Directa" te ha enviado un mensaje sobre tu participación en el evento del fin de semana.',
                timestamp: new Date(Date.now() - 12 * 60 * 60 * 1000), // 12 horas atrás
                read: true,
                type: 'message'
            },
            {
                id: 6,
                title: 'Horas de voluntariado registradas',
                description: 'Se han registrado 4 horas de voluntariado por tu participación en la campaña de limpieza de playas.',
                timestamp: new Date(Date.now() - 24 * 60 * 60 * 1000), // 1 día atrás
                read: true,
                type: 'hours'
            },
            {
                id: 7,
                title: 'Nuevo logro desbloqueado',
                description: '¡Felicitaciones! Has desbloqueado el logro "Voluntario Comprometido" por completar 50 horas de servicio.',
                timestamp: new Date(Date.now() - 2 * 24 * 60 * 60 * 1000), // 2 días atrás
                read: false,
                type: 'achievement'
            },
            {
                id: 8,
                title: 'Evento cancelado',
                description: 'El evento de voluntariado en el Parque Zoológico Nacional ha sido cancelado debido al mal tiempo.',
                timestamp: new Date(Date.now() - 3 * 24 * 60 * 60 * 1000), // 3 días atrás
                read: true,
                type: 'cancellation'
            }
        ];
    }

    setupEventListeners() {
        // Botón de notificaciones
        const notificationBtn = document.getElementById('notificationBtn');
        if (notificationBtn) {
            notificationBtn.addEventListener('click', () => {
                this.openNotificationModal();
            });
        }

        // Filtros de notificaciones
        const filterInputs = document.querySelectorAll('input[name="notificationFilter"]');
        filterInputs.forEach(input => {
            input.addEventListener('change', (e) => {
                this.currentFilter = e.target.value;
                this.renderNotifications();
            });
        });

        // Botón marcar todas como leídas
        const markAllReadBtn = document.getElementById('markAllReadBtn');
        if (markAllReadBtn) {
            markAllReadBtn.addEventListener('click', () => {
                this.markAllAsRead();
            });
        }

        // Evento para cerrar modal
        const modal = document.getElementById('notificationModal');
        if (modal) {
            modal.addEventListener('hidden.bs.modal', () => {
                this.currentFilter = 'all';
                const filterAll = document.getElementById('filterAll');
                if (filterAll) filterAll.checked = true;
            });
        }
    }

    openNotificationModal() {
        const modal = new bootstrap.Modal(document.getElementById('notificationModal'));
        modal.show();
        this.renderNotifications();
    }

    getFilteredNotifications() {
        switch (this.currentFilter) {
            case 'unread':
                return this.notifications.filter(n => !n.read);
            case 'read':
                return this.notifications.filter(n => n.read);
            default:
                return this.notifications;
        }
    }

    renderNotifications() {
        const container = document.getElementById('notificationsList');
        const noNotifications = document.getElementById('noNotifications');
        const filteredNotifications = this.getFilteredNotifications();

        if (!container) return;

        if (filteredNotifications.length === 0) {
            container.innerHTML = '';
            if (noNotifications) noNotifications.style.display = 'block';
            return;
        }

        if (noNotifications) noNotifications.style.display = 'none';

        container.innerHTML = filteredNotifications
            .sort((a, b) => b.timestamp - a.timestamp)
            .map(notification => this.createNotificationHTML(notification))
            .join('');

        // Agregar event listeners a los botones de las notificaciones
        this.setupNotificationEventListeners();
    }

    createNotificationHTML(notification) {
        const timeAgo = this.getTimeAgo(notification.timestamp);
        const statusClass = notification.read ? 'read' : 'unread';
        const statusDot = notification.read ? 'read' : 'unread';
        const iconClass = this.getNotificationIcon(notification.type);

        return `
            <div class="notification-item ${statusClass}" data-id="${notification.id}">
                <div class="notification-header">
                    <h6 class="notification-title">
                        <span class="notification-status ${statusDot}"></span>
                        <i class="${iconClass} me-2"></i>
                        ${notification.title}
                    </h6>
                    <span class="notification-time">${timeAgo}</span>
                </div>
                <div class="notification-description">
                    ${notification.description}
                </div>
                <div class="notification-actions">
                    ${!notification.read ? `
                        <button class="btn btn-primary btn-sm mark-read-btn" data-id="${notification.id}">
                            <i class="bi bi-check me-1"></i>Marcar como leída
                        </button>
                    ` : ''}
                    <button class="btn btn-outline-secondary btn-sm view-more-btn" data-id="${notification.id}">
                        <i class="bi bi-eye me-1"></i>Ver más
                    </button>
                </div>
            </div>
        `;
    }

    getNotificationIcon(type) {
        const icons = {
            'event': 'bi bi-calendar-event',
            'reminder': 'bi bi-clock',
            'profile': 'bi bi-person',
            'opportunity': 'bi bi-lightbulb',
            'message': 'bi bi-chat',
            'hours': 'bi bi-clock-history',
            'achievement': 'bi bi-trophy',
            'cancellation': 'bi bi-x-circle'
        };
        return icons[type] || 'bi bi-bell';
    }

    getTimeAgo(timestamp) {
        const now = new Date();
        const diff = now - timestamp;
        const minutes = Math.floor(diff / (1000 * 60));
        const hours = Math.floor(diff / (1000 * 60 * 60));
        const days = Math.floor(diff / (1000 * 60 * 60 * 24));

        if (minutes < 1) return 'Ahora mismo';
        if (minutes < 60) return `Hace ${minutes} minuto${minutes > 1 ? 's' : ''}`;
        if (hours < 24) return `Hace ${hours} hora${hours > 1 ? 's' : ''}`;
        if (days < 7) return `Hace ${days} día${days > 1 ? 's' : ''}`;
        return timestamp.toLocaleDateString('es-ES');
    }

    setupNotificationEventListeners() {
        // Botones marcar como leída
        const markReadBtns = document.querySelectorAll('.mark-read-btn');
        markReadBtns.forEach(btn => {
            btn.addEventListener('click', (e) => {
                const id = parseInt(e.target.closest('.mark-read-btn').dataset.id);
                this.markAsRead(id);
            });
        });

        // Botones ver más
        const viewMoreBtns = document.querySelectorAll('.view-more-btn');
        viewMoreBtns.forEach(btn => {
            btn.addEventListener('click', (e) => {
                const id = parseInt(e.target.closest('.view-more-btn').dataset.id);
                this.viewMore(id);
            });
        });
    }

    markAsRead(id) {
        const notification = this.notifications.find(n => n.id === id);
        if (notification) {
            notification.read = true;
            this.updateNotificationCount();
            this.renderNotifications();
            this.showToast('Notificación marcada como leída', 'success');
        }
    }

    markAllAsRead() {
        this.notifications.forEach(notification => {
            notification.read = true;
        });
        this.updateNotificationCount();
        this.renderNotifications();
        this.showToast('Todas las notificaciones marcadas como leídas', 'success');
    }

    viewMore(id) {
        const notification = this.notifications.find(n => n.id === id);
        if (notification) {
            this.showToast(`Redirigiendo a detalles de: ${notification.title}`, 'info');
            // Aquí se podría implementar la redirección real
            setTimeout(() => {
                console.log(`Redirigiendo a detalles de notificación ${id}`);
            }, 1000);
        }
    }

    updateNotificationCount() {
        const countElement = document.getElementById('notificationCount');
        const dotElement = document.getElementById('notificationDot');
        const unreadCount = this.notifications.filter(n => !n.read).length;
        
        if (countElement) {
            if (unreadCount > 0) {
                countElement.textContent = unreadCount > 99 ? '99+' : unreadCount;
                countElement.style.display = 'flex';
            } else {
                countElement.style.display = 'none';
            }
        }
        
        // Manejar el punto rojo
        if (dotElement) {
            if (unreadCount > 0) {
                dotElement.style.display = 'block';
            } else {
                dotElement.style.display = 'none';
            }
        }
    }

    showToast(message, type = 'info') {
        // Crear toast simple
        const toastContainer = document.getElementById('toastContainer') || this.createToastContainer();
        
        const toast = document.createElement('div');
        toast.className = `toast align-items-center text-white bg-${type === 'success' ? 'success' : type === 'error' ? 'danger' : 'primary'} border-0`;
        toast.setAttribute('role', 'alert');
        toast.setAttribute('aria-live', 'assertive');
        toast.setAttribute('aria-atomic', 'true');
        
        toast.innerHTML = `
            <div class="d-flex">
                <div class="toast-body">
                    ${message}
                </div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>
        `;
        
        toastContainer.appendChild(toast);
        
        const bsToast = new bootstrap.Toast(toast);
        bsToast.show();
        
        // Remover toast después de que se oculte
        toast.addEventListener('hidden.bs.toast', () => {
            toast.remove();
        });
    }

    createToastContainer() {
        const container = document.createElement('div');
        container.id = 'toastContainer';
        container.className = 'toast-container position-fixed top-0 end-0 p-3';
        container.style.zIndex = '9999';
        document.body.appendChild(container);
        return container;
    }

    addNotification(notification) {
        notification.id = Math.max(...this.notifications.map(n => n.id), 0) + 1;
        notification.timestamp = new Date();
        notification.read = false;
        this.notifications.unshift(notification);
        this.updateNotificationCount();
        this.renderNotifications();
        this.showToast('Nueva notificación recibida', 'info');
    }
}

// Inicializar el sistema de notificaciones cuando el DOM esté listo
document.addEventListener('DOMContentLoaded', () => {
    window.notificationSystem = new NotificationSystem();
    
    // Simular nuevas notificaciones cada 30 segundos (solo para demo)
    setInterval(() => {
        const mockNotifications = [
            {
                title: 'Nuevo evento disponible',
                description: 'Se ha agregado un nuevo evento de voluntariado en tu área. ¡Revisa los detalles!',
                type: 'event'
            },
            {
                title: 'Recordatorio importante',
                description: 'No olvides tu sesión de voluntariado programada para mañana.',
                type: 'reminder'
            },
            {
                title: 'Mensaje de la comunidad',
                description: 'Tienes un nuevo mensaje de la comunidad de voluntarios.',
                type: 'message'
            }
        ];
        
        const randomNotification = mockNotifications[Math.floor(Math.random() * mockNotifications.length)];
        window.notificationSystem.addNotification(randomNotification);
    }, 30000); // 30 segundos
}); 