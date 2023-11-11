
$(function () {
    $(document).on("click", ".btn", function (e) {
        e.preventDefault();

        var user = {
            Name: $(".txtName").val(),
            Surname: $(".txtSurname").val(),
            Username: $(".txtUsername").val(),
            Password: $(".txtPassword").val(),
        }

        CallRequest("/Login/Register", user, function (rsp) {
            if (rsp.IsSuccess) {
                window.location.replace("/Dashboard");
            }
        });

    });
});