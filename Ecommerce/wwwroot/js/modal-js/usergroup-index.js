(function ($) {
    function UserGroup() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-usergroup").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new UserGroup();
        self.init();
    })
}(jQuery))