(function ($) {
    function Unit() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-unit").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new Unit();
        self.init();
    })
}(jQuery))