(function ($) {
    function Agency() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-agency").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new Agency();
        self.init();
    })
}(jQuery))