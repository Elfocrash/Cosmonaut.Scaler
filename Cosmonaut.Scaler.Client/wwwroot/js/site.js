function closeAddModal() {
	$('#addAccountModal').modal('hide');
}

function successToast(message) {
	toastr["success"](message);
}

function failureToast(message) {
    toastr["error"](message);
}