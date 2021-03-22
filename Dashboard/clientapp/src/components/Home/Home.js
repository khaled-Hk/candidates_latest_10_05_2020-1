

export default {
    name: 'home',
    components: {
      
    },
    created() {
        this.getStatistics();
    },
    data() {
        return {
            statistics : {}
        };
    },
    methods: {
        getStatistics() {
            this.$blockUI.Start();
            this.$http.GetStatistics().then(response => {
                this.$blockUI.Stop();
                this.statistics = response.data;
                this.$blockUI.Stop();
            }).catch(() => {
                this.$blockUI.Stop();
            })
        }
    }    
}
