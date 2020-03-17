(function ($) {
    function Slider() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-Slider").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new Slider();
        self.init();
    })
}(jQuery))