<template>
    <div style="background-color: rgb(92, 134, 151)">
        <img alt="Vue logo" src="./assets/Alektum-A.png">
    </div>
    <HelloWorld msg="Welcome to Your Vue.js App" />
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

<!-- Kallas p� i HelloWorld.vue -->
<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
}
</style>
