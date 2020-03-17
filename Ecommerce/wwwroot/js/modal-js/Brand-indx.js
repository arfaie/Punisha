(function ($) {
    function Brand() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-Brand").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new Brand();
        self.init();
    })
}(jQuery))