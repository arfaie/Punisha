(function ($) {
    function Car() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-car").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new Car();
        self.init();
    })
}(jQuery))