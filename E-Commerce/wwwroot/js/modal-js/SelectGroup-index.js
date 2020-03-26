(function ($) {
    function SelectGroup() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-selectgroup").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new SelectGroup();
        self.init();
    })
}(jQuery))