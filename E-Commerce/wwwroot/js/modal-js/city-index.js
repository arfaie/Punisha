(function ($) {
    function City() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-city").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new City();
        self.init();
    })
}(jQuery))