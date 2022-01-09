/*36.1 Создаём функции доступа к toastr*/
window.ShowToastr = (type, message) => {
    if (type === "success") {
        toastr.success(message, "Operation Successful");
    }
    if (type === "error") {
        toastr.error(message, "Operation Failed");
    }
}

/*44. Добавляем функции вызова*/
window.ShowSweetAlerts = (type, message) => {
    if (type === "success") {
        Swal.fire(
            'Success Notification',
            message,
            'success'
        )
    }
    if (type === "error") {
        Swal.fire(
            'Error Notification',
            message,
            'error'
        )
    }

    if (type === "deleting") {
        Swal.fire(
            {
                title: 'Are you sure?',
                text: message,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, delete it!'
            }).then((result) => {
                if (result.isConfirmed) {
                    return true;
                }
                return false;
            })
    }
}