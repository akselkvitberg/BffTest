<script setup lang="ts">
import {ref} from 'vue'

defineProps<{ msg: string }>()

const isLoggedIn = ref(false)
fetch('/bff/user', {
  'headers': {
    'x-csrf': 1
  }
}).then(async (result:Response) => isLoggedIn.value = result.status === 200)

const weatherForecast = ref([])
fetch('/WeatherForecast', {
  'headers': {
    'x-csrf': 1 // todo: easier way to call
  }
}).then(async (result:Response) => weatherForecast.value = await result.json())

const logout = async () => {
  return await fetch('/bff/logout', {
    method: 'GET',
    'headers': {
      'x-csrf': 1 // todo: easier way to call
    }
  });
}


const count = ref(0)
</script>

<template>
  <h1>{{ msg }}</h1>

  <div class="card">
    <div>
      <a v-if="!isLoggedIn" href="/bff/login">Login</a>
      <a v-else @click="logout">Log out</a>
    </div>
    <button type="button" @click="count++">count is {{ count }}</button>
    <p>Logged in: {{ isLoggedIn }}</p>
    <p>
      Edit
      <code>components/HelloWorld.vue</code> to test HMR
    </p>
  </div>

  <div class="card" v-if="weatherForecast">
    <p>Weather forecast</p>
    <ul>
      <li v-for="forecast in weatherForecast" :key="forecast.date">
        {{ forecast.date }}: {{ forecast.summary }} ({{ forecast.temperatureC }} ℃)
      </li>
    </ul>
  </div>

  <p>
    Check out
    <a href="https://vuejs.org/guide/quick-start.html#local" target="_blank"
    >create-vue</a
    >, the official Vue + Vite starter
  </p>
  <p>
    Install
    <a href="https://github.com/vuejs/language-tools" target="_blank">Volar</a>
    in your IDE for a better DX
  </p>
  <p class="read-the-docs">Click on the Vite and Vue logos to learn more</p>
</template>

<style scoped>
.read-the-docs {
  color: #888;
}
</style>
