$(document).ready(function(){
       //    resposive-megamenu-mobile------------------
        $('.dropdown-toggle').on('click', function(e) {
            e.stopPropagation();
            e.preventDefault();

            var self = $(this);
            if (self.is('.disabled, :disabled')) {
              return false;
            }
            self.parent().toggleClass("open");
          });

          $(document).on('click', function(e) {
            if ($('.dropdown').hasClass('open')) {
              $('.dropdown').removeClass('open');
            }
          });

          $('.nav-btn.nav-slider').on('click', function() {
            $('.overlay').show();
            $('nav').toggleClass("open");
          });

          $('.overlay').on('click', function() {
            if ($('nav').hasClass('open')) {
              $('nav').removeClass('open');
            }
            $(this).hide();
          });
    
    
            $('li.active').addClass('open').children('ul').show();
            $("li.has-sub > a").on('click', function () {
                $(this).removeAttr('href');
                var e = $(this).parent('li');
                if (e.hasClass('open')) {
                    e.removeClass('open');
                    e.find('li').removeClass('opne');
                    e.find('ul').slideUp(200);
                }
                else {
                    e.addClass('open');
                    e.children('ul').slideDown(200);
                    e.siblings('li').children('ul').slideUp(200);
                    e.siblings('li').removeClass('open');
                    e.siblings('li').find('li').removeClass('open');
                    e.siblings('li').find('ul').slideUp(200);
                }
            });
//    resposive-megamenu-mobile------------------
    
//    countdown----------------------------
    ! function (l) {
    var t = {
            init: function () { t.countDown()
            },
            countDown: function (t, i) {
                l(".countdown").each(function () {
                    var t = l(this),
                        a = l(this).data("date-time"),
                        e = l(this).data("labels");
                    (i || t).countdown(a, function (t) {
                        l(this).html(t.strftime('<div class="countdown-item"><div class="countdown-value">%D</div><div class="countdown-label">' + e["label-day"] + '</div></div><div class="countdown-item"><div class="countdown-value">%H</div><div class="countdown-label">' + e["label-hour"] + '</div></div><div class="countdown-item"><div class="countdown-value">%M</div><div class="countdown-label">' + e["label-minute"] + '</div></div><div class="countdown-item"><div class="countdown-value">%S</div><div class="countdown-label">' + e["label-second"] + "</div></div>"))
                    })
                })
            },
        };
    l(function () {
        t.init()
    })
}(jQuery);
//    countdown----------------------------
    
//    tab---------------------------------
    $(".checkout-tab-pill").click(function(){
    var index = $(this).index();
    $(".checkout-tab-pill").removeClass("listing-active-cart");
    $(this).addClass("listing-active-cart");
    $(".cart-tab-main").slideUp(0);
    $(".cart-tab-main").eq(index).slideDown(0);
    
});
    
    $("ul.listing-sort li").click(function(){
    var index = $(this).index();
    $("ul.listing-sort li").removeClass("listing-active");
    $(this).addClass("listing-active");
    $("ul.listing-item li").slideUp(0);
    $("ul.listing-item li").eq(index).slideDown(0);
    
});
    
    $(".box-header-sidebar").on('click',function(e){
    e.preventDefault();
    $(".box-header-sidebar").removeClass("activeacc");
    $(this).addClass("activeacc");
    $(this).next().slideToggle(200);
});   
    $(".checkout-order-summary-header").on('click',function(){
    $(this).next().slideToggle(200);
});  
    
    $("li.box-tabs-tab").click(function(e){
    e.preventDefault();
    var index = $(this).index();
    $("li.box-tabs-tab").removeClass("active-tabs");
    $(this).addClass("active-tabs");
    $(".tab-active-content .tab").slideUp(0);
    $(".tab-active-content .tab").eq(index).slideDown(0);
    
}); 
    
    $("ul.filter-items > li").click(function(e){
    e.preventDefault();
    var index = $(this).index();
    $("ul.filter-items > li").removeClass("filter-items-active");
    $(this).addClass("filter-items-active");
    $("ul.comments-list > li").slideUp(0);
    $("ul.comments-list > li").eq(index).slideDown(0);
    
});
    
    $("ul.faq-filter-items li").click(function(e){
    e.preventDefault();
    var index = $(this).index();
    $("ul.faq-filter-items li").removeClass("filter-items-active");
    $(this).addClass("filter-items-active");
    $("ul.faq-list > li").slideUp(0);
    $("ul.faq-list > li").eq(index).slideDown(0);
    
});
    //    tab---------------------------------
    
//    modal-------------------------
        var modal = document.getElementById("modal");
        window.onclick = (function(event){
        if(event.target == modal){
            modal.style.display = 'none';
        }
        }); 
    
//    modal-------------------------
    
    //    Scroll---------------------------
    $(document).on("scroll", function () {
        var st = $(this).scrollTop();
        if (st > 10) {
            $(".footer-jump-angle").fadeIn(0, "swing");
        }
        else if (st < 300) {
            $(".footer-jump-angle").fadeOut(0, "swing");
        }
    });
    $(".footer-jump-angle").on("click", function () {
        $("html,body").animate({scrollTop: "0px"}, 3000, "swing");
    });
    $(document).scroll(function () {
        var scroll = $(document).scrollTop();

        if (scroll > 200) {
            $(".main-menu").addClass("NavFix");
        } else if (scroll < 10) {
            $(".main-menu").removeClass("NavFix");
        }
    });
    
//    Scroll--------------------------- 
    
//    quantity-selector---------------------------
    jQuery('<div class="quantity-nav"><div class="quantity-button quantity-up">+</div><div class="quantity-button quantity-down">-</div></div>').insertAfter('.quantity input');
    jQuery('.quantity').each(function() {
      var spinner = jQuery(this),
        input = spinner.find('input[type="number"]'),
        btnUp = spinner.find('.quantity-up'),
        btnDown = spinner.find('.quantity-down'),
        min = input.attr('min'),
        max = input.attr('max');

      btnUp.click(function() {
        var oldValue = parseFloat(input.val());
        if (oldValue >= max) {
          var newVal = oldValue;
        } else {
          var newVal = oldValue + 1;
        }
        spinner.find("input").val(newVal);
        spinner.find("input").trigger("change");
      });

      btnDown.click(function() {
        var oldValue = parseFloat(input.val());
        if (oldValue <= min) {
          var newVal = oldValue;
        } else {
          var newVal = oldValue - 1;
        }
        spinner.find("input").val(newVal);
        spinner.find("input").trigger("change");
      });

    });
//    quantity-selector---------------------------
    
//    checkbox--------------------------
        $(".remember-checkbox").click(function(){
        if($(this).is(':checked')){
             $(this).parents('.checkbox-primary').find('.checkbox-check').addClass("checkbox-custom-pic");
            
        }else{
             $(this).parents('.checkbox-primary').find('.checkbox-check').removeClass("checkbox-custom-pic");
            
        }
    });
//    checkbox--------------------------
    
//    verify-phone-number--------------------
        if($("#countdown-verify-end").length) {
        var $countdownOptionEnd = $("#countdown-verify-end");

        $countdownOptionEnd.countdown({
        date: (new Date()).getTime() + 180 * 1000, // 1 minute later
        text: '<span class="day">%s</span><span class="hour">%s</span><span>: %s</span><span>%s</span>',
        end: function() {
            $countdownOptionEnd.html("<a href='' class='link-border-verify form-account-link'>ارسال مجدد</a>");
        }
        });
        }
    $(".line-number-account").keyup(function(){
            $(this).next().focus();
        });
    //    verify-phone-number--------------------
    
    // favorites product----------------------
    
    $("ul.gallery-options button.btn-option-wishes").on("click",function () {
        $(this).toggleClass("btn-option-favorites");
    });
    
    // favorites product-----------------------
    
//single-product------------------
    $("#gallery-slider").owlCarousel({
        rtl: true,
        margin: 10,
        nav: true,
        navText: ['<i class="fa fa-angle-right"></i>', '<i class="fa fa-angle-left"></i>'],
        dots: false,
        responsiveClass: true,
        responsive: {
            0: {
                items: 4,
                slideBy: 1
            }
        }
    });

    $('.back-to-top').click(function (e) {
        e.preventDefault();
        $('html, body').animate({ scrollTop: 0 }, 800, 'easeInExpo');
    });

    if ($("#img-product-zoom").length) {
        $("#img-product-zoom").ezPlus({
            zoomType: "inner",
            containLensZoom: true,
            gallery: 'gallery_01f',
            cursor: "crosshair",
            galleryActiveClass: "active",
            responsive: true,
            imageCrossfade: true,
            zoomWindowFadeIn: 500,
            zoomWindowFadeOut: 500
        });
    }
//single-product------------------
    $(".product-params-more-handler a").on('click',function(e){
        e.preventDefault();
        $(".product-params-more").slideToggle(200);
        $(this).find('.show-more').fadeToggle(0);
        $(this).find('.show-less').fadeToggle(0);
    });
    
    $(".table-suppliers-more a").on('click',function(e){
        e.preventDefault();
        $(".in-list").slideToggle(200);
        $(this).find('.show-more').fadeToggle(0);
        $(this).find('.show-less').fadeToggle(0);
    });
    
    $(".mask-handler").click(function (e) {
        e.preventDefault();
        var sumaryBox = $(this).parents('.content-expert-summary');
        sumaryBox.find('.mask-text-product-summary').toggleClass('active');
        sumaryBox.find('.shadow-box').fadeToggle();
        $(this).find('.show-more').fadeToggle(0);
        $(this).find('.show-less').fadeToggle(0);
    });
    
        $(".expert-article-button").click(function (e) {
        e.preventDefault();
        var sumaryBox = $(this).parents('.js-expert-article');
        sumaryBox.find('.js-expert-article').toggleClass('active');
        sumaryBox.find('.content-expert-text').slideToggle();
        $(this).find('.show-more').fadeToggle(0);
        $(this).find('.show-less').fadeToggle(0);
    });
    //start slider sidebar-----------------------------
    $("#suggestion-slider").owlCarousel({
        rtl: true,
        items: 1,
        autoplay: true,
        autoplayTimeout: 5000,
        loop: true,
        dots: false,
        onInitialized: startProgressBar,
        onTranslate: resetProgressBar,
        onTranslated: startProgressBar
    });
    
    function startProgressBar() {
      $(".slide-progress").css({
        width: "100%",
        transition: "width 5000ms"
      });
    }

    function resetProgressBar() {
      $(".slide-progress").css({
        width: 0,
        transition: "width 0s"
      });
    }
    //start slider sidebar-----------------------------
    
    //    slider-product-------------------
    $(".product-carousel").owlCarousel({
        rtl: true,
        margin: 10,
        nav: true,
        navText: ['<i class="fa fa-angle-right"></i>', '<i class="fa fa-angle-left"></i>'],
        dots: false,
        responsiveClass: true,
        responsive: {
            0: {
                items: 2,
                slideBy: 1
            },
            576: {
                items: 2,
                slideBy: 1
            },
            768: {
                items: 3,
                slideBy: 2
            },
            992: {
                items: 4,
                slideBy: 2
            },
            1400: {
                items: 4,
                slideBy: 3
            }
        }
    });
    //    slider-product------------------- 
});
    




