$(document).ready(function () {
    var $select2 = $('#searchBox').select2({
        //language: "fa-IR",
        dir: "rtl",
        placeholder: "جستجو در کاربیوتیک...",
        allowClear: true,
        language: {
            noResults: function (params) {
                return "";
            },
            searching: function () {
                return "در حال جستجو...";
            }
        },
        ajax: {
            delay: 250,
            url: '/Product/SearchBox',
            dataType: 'json',
            minimumInputLength: 1,
            width: 'resolve',
            data: function (params) {
                return {
                    search: params.term
                };
            },
            processResults: function (data) {
                return {
                    results: data.items
                };
            },
        }
    });

    $select2.on('select2:selecting', function (e) {
        var args = e.params.args;

        if (args.data.id.startsWith("!")) {
            var categoryGroupId = args.data.id.replace("!", "");

            window.location = '/Product/CategoryGroup' + '/' + categoryGroupId;
        }
        else if (args.data.id.startsWith("@")) {
            var categoryId = args.data.id.replace("@", "");

            window.location = '/Product/Category' + '/' + categoryId;
        }
        else if (args.data.id.startsWith("#")) {
            var makerId = args.data.id.replace("#", "");

            window.location = '/Product/Maker' + '/' + makerId;
        }
        else if (args.data.id.startsWith("$")) {
            var carId = args.data.id.replace("$", "");

            window.location = '/Product/Car' + '/' + carId;
        }
        else if (args.data.id.startsWith("%")) {
            var brandId = args.data.id.replace("%", "");

            window.location = '/Product/Brand' + '/' + brandId;
        }
        else {
            window.location = '/Product/Details' + '/' + args.data.id;
        }
    });
});