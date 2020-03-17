(function ($) {
    function SelectItem() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-selectitem").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new SelectItem();
        self.init();
    })
}(jQuery))