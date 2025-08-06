// Sistema de notificaciones toast no invasivas
class ToastManager {
    constructor() {
        this.createToastContainer();
    }

    createToastContainer() {
        if (!document.getElementById('toast-container')) {
            const container = document.createElement('div');
            container.id = 'toast-container';
            container.className = 'toast-container position-fixed top-0 end-0 p-3';
            container.style.zIndex = '1055';
            document.body.appendChild(container);
        }
    }

    show(message, type = 'success', duration = 5000) {
        const toastId = 'toast-' + Date.now();
        const iconClass = type === 'success' ? 'bi-check-circle-fill' : 'bi-exclamation-triangle-fill';
        const bgClass = type === 'success' ? 'bg-success' : 'bg-danger';
        
        const toastHtml = `
            <div id="${toastId}" class="toast" role="alert" aria-live="assertive" aria-atomic="true">
                <div class="toast-header ${bgClass} text-white">
                    <i class="${iconClass} me-2"></i>
                    <strong class="me-auto">${type === 'success' ? 'Éxito' : 'Error'}</strong>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="toast" aria-label="Cerrar"></button>
                </div>
                <div class="toast-body">
                    ${message}
                </div>
            </div>
        `;

        const container = document.getElementById('toast-container');
        container.insertAdjacentHTML('beforeend', toastHtml);

        const toastElement = document.getElementById(toastId);
        const toast = new bootstrap.Toast(toastElement, {
            autohide: true,
            delay: duration
        });

        toast.show();

        // Remover del DOM después de que se oculte
        toastElement.addEventListener('hidden.bs.toast', function () {
            toastElement.remove();
        });
    }

    success(message, duration = 5000) {
        this.show(message, 'success', duration);
    }

    error(message, duration = 7000) {
        this.show(message, 'error', duration);
    }
}

// Instancia global
const Toast = new ToastManager();

// Función helper para formularios AJAX
function handleFormSubmission(form, successCallback = null) {
    const formData = new FormData(form);
    const actionUrl = form.action;

    // Deshabilitar botón de submit
    const submitBtn = form.querySelector('button[type="submit"]');
    const originalText = submitBtn.innerHTML;
    submitBtn.disabled = true;
    submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Guardando...';

    fetch(actionUrl, {
        method: 'POST',
        body: formData,
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        }
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            Toast.success(data.message);
            if (data.redirectUrl) {
                setTimeout(() => {
                    window.location.href = data.redirectUrl;
                }, 1500);
            }
            if (successCallback) {
                successCallback(data);
            }
        } else {
            Toast.error(data.message);
            if (data.errors && data.errors.length > 0) {
                data.errors.forEach(error => {
                    Toast.error(error, 5000);
                });
            }
        }
    })
    .catch(error => {
        console.error('Error:', error);
        Toast.error('Error de conexión. Por favor, inténtalo de nuevo.');
    })
    .finally(() => {
        // Rehabilitar botón
        submitBtn.disabled = false;
        submitBtn.innerHTML = originalText;
    });
}

// Auto-inicializar cuando el DOM esté listo
document.addEventListener('DOMContentLoaded', function() {
    // Interceptar todos los formularios con la clase 'ajax-form'
    document.querySelectorAll('form.ajax-form').forEach(form => {
        form.addEventListener('submit', function(e) {
            e.preventDefault();
            handleFormSubmission(this);
        });
    });
});