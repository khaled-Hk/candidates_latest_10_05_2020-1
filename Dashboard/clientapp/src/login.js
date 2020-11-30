import Vue from 'vue';
import VueI18n from 'vue-i18n'
import VueRouter from 'vue-router';
// ************ import elementui + css *****************
import ElementUI from 'element-ui';
require('./../node_modules/element-ui/lib/theme-chalk/index.css');
//*************************************************
//import Vuetify from 'vuetify'
import locale from 'element-ui/lib/locale/lang/en'
import BlockUIService from './Shared/BlockUIService.js';
import Login from './Login/Login.vue';

import DataService from './Shared/DataService';




//Vue.use(Vuetify)
Vue.use(VueI18n);
Vue.use(VueRouter);
Vue.use(ElementUI, { locale });

Vue.config.productionTip = false;

Vue.prototype.$http = DataService;
Vue.prototype.$blockUI = BlockUIService;


export const eventBus = new Vue();

//const i18n = new VueI18n({
//    locale: 'ar', // set locale
//    messages, // set locale messages
//})

const router = new VueRouter({
    mode: 'history',
    base: __dirname,
    linkActiveClass: 'active',
    routes: [
        { path: '/Login', component: Login }
    ]

});


Vue.filter('toUpperCase', function (value) {
    if (!value) return '';
    return value.toUpperCase();
});

new Vue({
    router,
    render: h => {
        return h(Layout);
    }
}).$mount('#app');
