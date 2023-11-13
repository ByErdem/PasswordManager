
function CallRequest(opt) {

    var token = localStorage.getItem("guid");

    var settings = {
        url: opt.link,
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        success: function (rsp) {
            if (opt.object != null) {
                opt.event(rsp, null, opt.object);
            } else {
                opt.event(rsp);
            }
        },
        error: function (req, status, error) {
            console.error("Error:", status, error);
            if (opt.object != null) {
                opt.event(null, error, opt.object);
            } else {
                opt.event(null, error);
            }
        }
    };

    // Token varsa, 'beforeSend' fonksiyonunu ayarlar
    if (opt.tokenNeeded) {
        if (token) {
            settings.beforeSend = function (xhr) {
                xhr.setRequestHeader('Authorization', 'Bearer ' + token);
            };
        }
    }


    if (opt.data) {
        settings.type = "POST";
        settings.data = JSON.stringify(opt.data);
    }

    $.ajax(settings);
}

function Login(user) {
    var settings = {
        link: "/Login/SignIn",
        data: user,
        object: null,
        tokenNeeded: false,
        event: function (rsp) {
            if (rsp.ResultStatus == 0) {
                localStorage.setItem("guid", rsp.Data.GuidKey)
                window.location.replace("/MyPasswords");
            }
            else {
                swal("Uyarı", rsp.ErrorMessage, "error");
            }
        }
    }

    CallRequest(settings);
}

function GetUserInformations() {
    var settings = {
        link: "/Login/SignIn",
        data: user,
        object: null,
        tokenNeeded: false,
        event: function (rsp) {
            if (rsp.ResultStatus == 0) {
                localStorage.setItem("guid", rsp.Data.GuidKey)
                window.location.replace("/MyPasswords");
            }
            else {
                swal("Uyarı", rsp.ErrorMessage, "error");
            }
        }
    }

    CallRequest(settings);
}

function checkTokenAndRedirect(url) {
    var token = localStorage.getItem('guid');

    if (!token) {
        window.location.href = '/Login'; // Token yoksa login sayfasına yönlendir.
    } else {
        window.location.href = url; // Token varsa belirtilen URL'ye yönlendir.
    }
}

function ConvertToBase64(input) {
    return btoa(unescape(encodeURIComponent(input)));
}


function UUIDV4() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0,
            v = c === 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

function ParseDate(date) {
    var timeStamp = date.match(/\d+/)[0];
    var timeStampNumber = parseInt(timeStamp, 10);
    var result = new Date(timeStampNumber);
    return FormatDate(result);
}

function FormatDate(date) {
    var day = date.getDate().toString().padStart(2, '0');
    var month = (date.getMonth() + 1).toString().padStart(2, '0');
    var year = date.getFullYear();
    var hour = date.getHours().toString().padStart(2, '0');
    var minute = date.getMinutes().toString().padStart(2, '0');
    var seconds = date.getSeconds().toString().padStart(2, '0');

    return `${day}.${month}.${year} ${hour}:${minute}:${seconds}`;
}

function PrintCounts() {

    var ProductCount = $.find("#ProductCount");
    var CategoryCount = $.find("#CategoryCount");
    var UserCount = $.find("#UserCount");

    if (ProductCount.length != 0) {

        var settings = {
            link: "/Dashboard/GetCounts",
            data: null,
            object: null,
            tokenNeeded: true,
            event: function (rsp) {
                $(ProductCount[0]).text(rsp.Data.ProductCount)
                $(CategoryCount[0]).text(rsp.Data.CategoryCount)
                $(UserCount[0]).text(rsp.Data.UserCount)
            }
        }

        CallRequest(settings);
    }
}

function FillSelect2(id, event) {

    var settings = {
        link: "/Category/GetAll",
        data: null,
        object: null,
        tokenNeeded: true,
        event: function (rsp) {

            console.log(rsp);


            let _selectIsOpen = false;
            // const selectedAmount = {};
            const MATCH_AT_START_OF_WORD = false;

            if (rsp.ResultStatus == 0) {

                var $selectElement = $(id);

                // Select2'nin bulunduğu konumu al (ebeveyn elementi)
                var $container = $selectElement.parent();

                // Select2'yi yok et
                $selectElement.select2('destroy');

                // Select2 tarafından oluşturulan görsel arayüzü kaldır
                $selectElement.next('.select2-container').remove();

                // Önceki Select2 elementini DOM'dan kaldır
                $selectElement.remove();

                // Yeni bir <select> elementi oluştur
                var $newSelect = $("<select></select>").addClass("form-control").addClass("select2").addClass(id.replace(".", ""));
                $newSelect.attr("name", id.replace(".", ""));

                // Yeni <select> elementini eski elementin bulunduğu yere ekle
                $container.append($newSelect);

                // Yeni veri seti
                var newData = [
                    {
                        "id": "-1",
                        "text": "Seçiniz"
                    }
                ];

                rsp.Data.forEach(function (item, index) {
                    newData.push({
                        "id": item.CATEGORYID,
                        "text": item.CATEGORYNAME
                    });
                });

                $newSelect.addClass(id);

                $newSelect.on('select2:open', function (e) {
                    // Biraz gecikme ekleyerek, Select2'nin arama kutusunu DOM'a ekleme sürecini tamamlamasını bekleyin
                    setTimeout(function () {
                        // Select2'nin arama kutusuna odaklan
                        $('.select2-search__field').focus();
                    }, 0); // Gecikmeyi 0 olarak ayarlamak, bu işlemin "sonraki döngüde" gerçekleşmesini sağlar
                });

                const matchTextAndExtras = (params, item) => {
                    // `params.term` is the search term the user entered.
                    // `item.text` is the text that is displayed for the item object.
                    // `items.extras` is an optional array of extra values that should be matched if not match is found in `item.text`.
                    // console.debug("matchCustom(): params: %o; item %o", params, item);
                    let query = $.trim(params.term);
                    // If there is no search term, return all of the data.
                    if (!query || query.length < 2) {
                        $("body").addClass("noTerm");
                        // return null;
                        return item;
                    } else {
                        $("body").removeClass("noTerm");
                    }
                    // Do not display the item if there is no 'text' property (or if it's empty).
                    if (!$.trim(item.text)) {
                        return null;
                    }
                    query = query.replace(/([^0-9A-Z -])/gi, "\\$1");
                    if (MATCH_AT_START_OF_WORD) {
                        query = "\\b" + query;
                    }
                    // Try to match the search term, first in `item.text` and then in `item.extras`.
                    const queryRe = new RegExp(query, "ig");
                    if (item.text.search(queryRe) > -1) {
                        const modifiedItem = $.extend({}, item, true);
                        // Surround all matches of the query term with <span> elements.
                        modifiedItem.text = item.text.replace(queryRe, match => {
                            return "<mark>" + match + "</mark>";
                        });
                        return modifiedItem;
                    } else if (item.extras) {
                        const matchedExtra = item.extras.find(text => {
                            return text.search(queryRe) > -1;
                        });
                        if (matchedExtra) {
                            const modifiedItem = $.extend({}, item, true);
                            modifiedItem.text +=
                                " <i>[" +
                                matchedExtra.replace(queryRe, match => {
                                    return "<mark>" + match + "</mark>";
                                }) +
                                "]</i>";
                            return modifiedItem;
                        }
                    }
                    return null;
                };

                const renderItem = item => {
                    // console.log("renderItem(): item: %o", item);
                    if (item.text !== "") {
                        return $("<div>").html(item.text);
                    } else {
                        return "";
                    }
                };

                $newSelect.select2({
                    data: newData,
                    matcher: matchTextAndExtras,
                    dropdownParent: $container,
                    minimumInputLength: 0, // Don't use this: it prevents the dropdown from displaying any items until the user enters a query. Instead, matchCustom() contains logic to implement the same behavior.
                    selectOnClose: true,
                    //sorter: items => {
                    //    return items.reverse();
                    //},
                    templateResult: renderItem
                });

                $newSelect
                    .on("select2:open", event => {
                        // console.debug("open: ", event);
                        _selectIsOpen = true;
                    })
                    .on("select2:close", event => {
                        // console.debug("close: ", event);
                        _selectIsOpen = false;
                    })
                    // .on("select2:selecting", (event) => {
                    //   console.debug("select2:selecting: ", event);
                    //   selectedAmount[event.params.args.data.id] = window.prompt('Enter an amount');
                    // })
                    .on("select2:select", (event) => {
                        console.debug("select:select: selected item:", event.params.data);
                    });

                $newSelect
                    .on("select2:opening", event => {
                        // console.debug("opening:", event);
                    })
                    .on("keypress", event => {
                        // console.debug('keypress event: %o', event);
                        // console.debug('--> event.which: %d', event.which);
                        if (_selectIsOpen) {
                            return;
                        }
                        const charCode = event.which;
                        if (
                            !(event.altKey || event.ctrlKey || event.metaKey) &&
                            ((charCode >= 48 && charCode <= 57) ||
                                (charCode >= 65 && charCode <= 90) ||
                                (charCode >= 97 && charCode <= 122))
                        ) {
                            $newSelect.select2("open");
                            $("input.select2-search__field")
                                .eq(0)
                                .val(String.fromCharCode(charCode));
                        }
                    });

                if (event != undefined)
                    event();
            }
        }
    }

    CallRequest(settings);
}

function FillSelect2WithMainCategories(id, event, selectedEvent) {

    var settings = {
        link: "/Category/GetMainCategories",
        data: null,
        object: null,
        tokenNeeded: true,
        event: function (rsp) {
            let _selectIsOpen = false;
            // const selectedAmount = {};
            const MATCH_AT_START_OF_WORD = false;

            if (rsp.ResultStatus == 0) {

                var $selectElement = $(id);

                // Select2'nin bulunduğu konumu al (ebeveyn elementi)
                var $container = $selectElement.parent();

                // Select2'yi yok et
                $selectElement.select2('destroy');

                // Select2 tarafından oluşturulan görsel arayüzü kaldır
                $selectElement.next('.select2-container').remove();

                // Önceki Select2 elementini DOM'dan kaldır
                $selectElement.remove();

                // Yeni bir <select> elementi oluştur
                var $newSelect = $("<select></select>").addClass("form-control").addClass(id.replace(".", ""));
                $newSelect.attr("name", id.replace(".", ""));

                // Yeni <select> elementini eski elementin bulunduğu yere ekle
                $container.append($newSelect);

                // Yeni veri seti
                var newData = [
                    {
                        "id": "-1",
                        "text": "Seçiniz"
                    }
                ];

                rsp.Data.forEach(function (item, index) {
                    newData.push({
                        "id": item.CATEGORYID,
                        "text": item.CATEGORYNAME
                    });
                });

                $newSelect.addClass(id);

                $newSelect.on('select2:open', function (e) {
                    // Biraz gecikme ekleyerek, Select2'nin arama kutusunu DOM'a ekleme sürecini tamamlamasını bekleyin
                    setTimeout(function () {
                        // Select2'nin arama kutusuna odaklan
                        $('.select2-search__field').focus();
                    }, 0); // Gecikmeyi 0 olarak ayarlamak, bu işlemin "sonraki döngüde" gerçekleşmesini sağlar
                });

                const matchTextAndExtras = (params, item) => {
                    // `params.term` is the search term the user entered.
                    // `item.text` is the text that is displayed for the item object.
                    // `items.extras` is an optional array of extra values that should be matched if not match is found in `item.text`.
                    // console.debug("matchCustom(): params: %o; item %o", params, item);
                    let query = $.trim(params.term);
                    // If there is no search term, return all of the data.
                    if (!query || query.length < 2) {
                        $("body").addClass("noTerm");
                        // return null;
                        return item;
                    } else {
                        $("body").removeClass("noTerm");
                    }
                    // Do not display the item if there is no 'text' property (or if it's empty).
                    if (!$.trim(item.text)) {
                        return null;
                    }
                    query = query.replace(/([^0-9A-Z -])/gi, "\\$1");
                    if (MATCH_AT_START_OF_WORD) {
                        query = "\\b" + query;
                    }
                    // Try to match the search term, first in `item.text` and then in `item.extras`.
                    const queryRe = new RegExp(query, "ig");
                    if (item.text.search(queryRe) > -1) {
                        const modifiedItem = $.extend({}, item, true);
                        // Surround all matches of the query term with <span> elements.
                        modifiedItem.text = item.text.replace(queryRe, match => {
                            return "<mark>" + match + "</mark>";
                        });
                        return modifiedItem;
                    } else if (item.extras) {
                        const matchedExtra = item.extras.find(text => {
                            return text.search(queryRe) > -1;
                        });
                        if (matchedExtra) {
                            const modifiedItem = $.extend({}, item, true);
                            modifiedItem.text +=
                                " <i>[" +
                                matchedExtra.replace(queryRe, match => {
                                    return "<mark>" + match + "</mark>";
                                }) +
                                "]</i>";
                            return modifiedItem;
                        }
                    }
                    return null;
                };

                const renderItem = item => {
                    // console.log("renderItem(): item: %o", item);
                    if (item.text !== "") {
                        return $("<div>").html(item.text);
                    } else {
                        return "";
                    }
                };

                $newSelect.select2({
                    data: newData,
                    matcher: matchTextAndExtras,
                    dropdownParent: $container,
                    minimumInputLength: 0, // Don't use this: it prevents the dropdown from displaying any items until the user enters a query. Instead, matchCustom() contains logic to implement the same behavior.
                    selectOnClose: true,
                    //sorter: items => {
                    //    return items.reverse();
                    //},
                    templateResult: renderItem
                });

                $newSelect
                    .on("select2:open", event => {
                        // console.debug("open: ", event);
                        _selectIsOpen = true;
                    })
                    .on("select2:close", event => {
                        // console.debug("close: ", event);
                        _selectIsOpen = false;
                    })
                    // .on("select2:selecting", (event) => {
                    //   console.debug("select2:selecting: ", event);
                    //   selectedAmount[event.params.args.data.id] = window.prompt('Enter an amount');
                    // })
                    .on("select2:select", (event) => {
                        if (selectedEvent != undefined)
                            selectedEvent(event.params.data);
                        console.debug("select:select: selected item:", event.params.data);
                    });

                $newSelect
                    .on("select2:opening", event => {
                        // console.debug("opening:", event);
                    })
                    .on("keypress", event => {
                        // console.debug('keypress event: %o', event);
                        // console.debug('--> event.which: %d', event.which);
                        if (_selectIsOpen) {
                            return;
                        }
                        const charCode = event.which;
                        if (
                            !(event.altKey || event.ctrlKey || event.metaKey) &&
                            ((charCode >= 48 && charCode <= 57) ||
                                (charCode >= 65 && charCode <= 90) ||
                                (charCode >= 97 && charCode <= 122))
                        ) {
                            $newSelect.select2("open");
                            $("input.select2-search__field")
                                .eq(0)
                                .val(String.fromCharCode(charCode));
                        }
                    });

                if (event != undefined)
                    event();
            }
        }
    }

    CallRequest(settings);
}

function FillSelect2WithSubCategories(id, data, event, selectedEvent) {


    let _selectIsOpen = false;
    // const selectedAmount = {};
    const MATCH_AT_START_OF_WORD = false;

    var $selectElement = $(id);

    // Select2'nin bulunduğu konumu al (ebeveyn elementi)
    var $container = $selectElement.parent();

    // Select2'yi yok et
    $selectElement.select2('destroy');

    // Select2 tarafından oluşturulan görsel arayüzü kaldır
    $selectElement.next('.select2-container').remove();

    // Önceki Select2 elementini DOM'dan kaldır
    $selectElement.remove();

    // Yeni bir <select> elementi oluştur
    var $newSelect = $("<select></select>").addClass("form-control").addClass(id.replace(".", ""));
    $newSelect.attr("name", id.replace(".", ""));

    // Yeni <select> elementini eski elementin bulunduğu yere ekle
    $container.append($newSelect);

    // Yeni veri seti
    var newData = [
        {
            "id": "-1",
            "text": "Seçiniz"
        }
    ];

    console.log(data);

    data.forEach(function (item, index) {
        newData.push({
            "id": item.CATEGORYID,
            "text": item.CATEGORYNAME
        });
    });

    $newSelect.addClass(id);

    $newSelect.on('select2:open', function (e) {
        // Biraz gecikme ekleyerek, Select2'nin arama kutusunu DOM'a ekleme sürecini tamamlamasını bekleyin
        setTimeout(function () {
            // Select2'nin arama kutusuna odaklan
            $('.select2-search__field').focus();
        }, 0); // Gecikmeyi 0 olarak ayarlamak, bu işlemin "sonraki döngüde" gerçekleşmesini sağlar
    });

    const matchTextAndExtras = (params, item) => {
        // `params.term` is the search term the user entered.
        // `item.text` is the text that is displayed for the item object.
        // `items.extras` is an optional array of extra values that should be matched if not match is found in `item.text`.
        // console.debug("matchCustom(): params: %o; item %o", params, item);
        let query = $.trim(params.term);
        // If there is no search term, return all of the data.
        if (!query || query.length < 2) {
            $("body").addClass("noTerm");
            // return null;
            return item;
        } else {
            $("body").removeClass("noTerm");
        }
        // Do not display the item if there is no 'text' property (or if it's empty).
        if (!$.trim(item.text)) {
            return null;
        }
        query = query.replace(/([^0-9A-Z -])/gi, "\\$1");
        if (MATCH_AT_START_OF_WORD) {
            query = "\\b" + query;
        }
        // Try to match the search term, first in `item.text` and then in `item.extras`.
        const queryRe = new RegExp(query, "ig");
        if (item.text.search(queryRe) > -1) {
            const modifiedItem = $.extend({}, item, true);
            // Surround all matches of the query term with <span> elements.
            modifiedItem.text = item.text.replace(queryRe, match => {
                return "<mark>" + match + "</mark>";
            });
            return modifiedItem;
        } else if (item.extras) {
            const matchedExtra = item.extras.find(text => {
                return text.search(queryRe) > -1;
            });
            if (matchedExtra) {
                const modifiedItem = $.extend({}, item, true);
                modifiedItem.text +=
                    " <i>[" +
                    matchedExtra.replace(queryRe, match => {
                        return "<mark>" + match + "</mark>";
                    }) +
                    "]</i>";
                return modifiedItem;
            }
        }
        return null;
    };

    const renderItem = item => {
        // console.log("renderItem(): item: %o", item);
        if (item.text !== "") {
            return $("<div>").html(item.text);
        } else {
            return "";
        }
    };

    $newSelect.select2({
        data: newData,
        matcher: matchTextAndExtras,
        dropdownParent: $container,
        minimumInputLength: 0, // Don't use this: it prevents the dropdown from displaying any items until the user enters a query. Instead, matchCustom() contains logic to implement the same behavior.
        selectOnClose: true,
        //sorter: items => {
        //    return items.reverse();
        //},
        templateResult: renderItem
    });

    $newSelect
        .on("select2:open", event => {
            // console.debug("open: ", event);
            _selectIsOpen = true;
        })
        .on("select2:close", event => {
            // console.debug("close: ", event);
            _selectIsOpen = false;
        })
        // .on("select2:selecting", (event) => {
        //   console.debug("select2:selecting: ", event);
        //   selectedAmount[event.params.args.data.id] = window.prompt('Enter an amount');
        // })
        .on("select2:select", (event) => {
            if (selectedEvent != undefined)
                selectedEvent(event.params.data);
            console.debug("select:select: selected item:", event.params.data);
        });

    $newSelect
        .on("select2:opening", event => {
            // console.debug("opening:", event);
        })
        .on("keypress", event => {
            // console.debug('keypress event: %o', event);
            // console.debug('--> event.which: %d', event.which);
            if (_selectIsOpen) {
                return;
            }
            const charCode = event.which;
            if (
                !(event.altKey || event.ctrlKey || event.metaKey) &&
                ((charCode >= 48 && charCode <= 57) ||
                    (charCode >= 65 && charCode <= 90) ||
                    (charCode >= 97 && charCode <= 122))
            ) {
                $newSelect.select2("open");
                $("input.select2-search__field")
                    .eq(0)
                    .val(String.fromCharCode(charCode));
            }
        });

    if (event != undefined)
        event();

}

$(function () {

    if ($(".userName").length > 0) {
        console.log("doğru")

        var settings = {
            link: "/Login/GetUserInformations",
            data: null,
            object: null,
            tokenNeeded: true,
            event: function (rsp) {
                $(".userName").text(rsp.Data.Name + " " + rsp.Data.Surname);
            }
        }

        CallRequest(settings);

    }

    if ($(".Logout").length > 0) {

        $(document).on("click", ".Logout", function (e) {
            e.preventDefault();

            var settings = {
                link: "/Login/Logout",
                data: null,
                object: null,
                tokenNeeded: true,
                event: function (rsp) {
                    if (rsp.ResultStatus == 0 && rsp.Data == 1) {
                        window.location = "/Login";
                    }
                }
            }

            CallRequest(settings);

        });



        $(".Logout")
    }

    //Proje bittiğinde burası aktifleştirilecek
    //setInterval(function () {
    //    PrintCounts();
    //}, 10000);

});
