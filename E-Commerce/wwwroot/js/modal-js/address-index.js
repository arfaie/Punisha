(function ($) {
    function Addresses() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-address").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new Addresses();
        self.init();
    })
}(jQuery))