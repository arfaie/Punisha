$(document).ready(function () {
    $(".menu li.first").addClass('openul').children('ul').show();

    $(".menu li.main-li > a").on('click', function () {
        var getli = $(this).parent('li');
        if (getli.hasClass('openul')) {
            getli.removeClass('openul');
            getli.find('ul').slideUp(200);
        }
        else {
            getli.addClass('openul');
            getli.children('ul').slideDown(200);
        }
    })
})