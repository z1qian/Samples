(window["webpackJsonp"]=window["webpackJsonp"]||[]).push([["chunk-19f10931"],{e350:function(e,t,n){"use strict";n.d(t,"b",(function(){return i})),n.d(t,"a",(function(){return u}));n("d3b7"),n("caad"),n("2532");var r=n("4360");function i(e){if(e&&e instanceof Array&&e.length>0){var t=r["a"].getters&&r["a"].getters.roles,n=e,i=t.some((function(e){return n.includes(e)}));return i}return console.error("need roles! Like v-permission=\"['admin','editor']\""),!1}function u(e){if(e){var t=r["a"].getters&&r["a"].getters.code;return e===t}return console.error("部门编号为空"),!1}},f3ae:function(e,t,n){"use strict";n.r(t);var r=function(){var e=this,t=e.$createElement,n=e._self._c||t;return n("div",{staticClass:"app-container"},[n("el-breadcrumb",{attrs:{"separator-class":"el-icon-arrow-right"}},[n("el-breadcrumb-item",{attrs:{to:{path:"/suggest"}}},[e._v("建议列表")]),e.suggestDetail.length>0?n("el-breadcrumb-item",[e._v(e._s(e.suggestDetail))]):e._e()],1),n("br"),n("router-view")],1)},i=[],u=(n("b0c0"),n("e350")),a={name:"SuggestList",computed:{suggestDetail:function(){return"SuggestDetail"===this.$route.name?this.$route.query.id?"建议详情":"新增建议":""},isDangZhengBan:function(){return Object(u["b"])(["dzb"])},isRenDaDaiBiao:function(){return Object(u["b"])(["rd"])},isLianLuoYuan:function(){return Object(u["b"])(["Liaison"])},isRenDaiLian:function(){return Object(u["b"])(["Committee","Allocation"])},isBanLiDanWei:function(){return Object(u["b"])(["Process"])},isAdmin:function(){return Object(u["b"])(["admin"])}},mounted:function(){}},s=a,o=n("2877"),c=Object(o["a"])(s,r,i,!1,null,null,null);t["default"]=c.exports}}]);