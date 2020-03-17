(function ($) {
    function state() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-state").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new state();
        self.init();
    })
}(jQuery))