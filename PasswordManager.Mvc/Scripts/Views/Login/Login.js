
$(function () {

    $(document).on("click", ".btn", function (e) {
        e.preventDefault();

        var user = {
            Username: $(".txtUsername").val(),
            Password: $(".txtPassword").val()
        }

        Login(user);

    });
});