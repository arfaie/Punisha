(function ($) {
    function Field() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-field").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }

    $(function () {
        var self = new Field();
        self.init();
    });
}(jQuery))