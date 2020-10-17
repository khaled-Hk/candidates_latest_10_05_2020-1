
export default (function () {
    var blockService = null;
    return {
        $loading: null,
        Start() {
            blockService = this.$loading({
                fullscreen: true,
                text: 'تـحميل البيـانات'
            });
        },
        Stop() {
            blockService.close();
        }
    };
})();



