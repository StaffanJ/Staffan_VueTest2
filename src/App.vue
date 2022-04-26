<template>
  <img alt="Vue logo" src="./assets/logo.png">
  <HelloWorld msg="Welcome to Your Vue.js App"/>
</template>

<script>
    import HelloWorld from './components/HelloWorld.vue'
    import employees from './css/custom/employees.css'
    import bootStrapCss from './css/boostrap/css/bootstrap.min.css'

export default {
  name: 'App',
  components: {
    HelloWorld
  }
}
</script>

<!-- Kallas på i HelloWorld.vue -->
<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
