(function ($) {
    function Maker() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-maker").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new Maker();
        self.init();
    })
}(jQuery))