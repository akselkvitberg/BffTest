import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import mkcert from 'vite-plugin-mkcert'

// https://vitejs.dev/config/
export default defineConfig({
  server:{
    port: 12001,
    https: true,
    hmr: {
      port: 12001,
      host: 'localhost',
      protocol: 'wss'
    }
  },
  
  build: {
    target: 'esnext'
  },
    
  plugins: [
      vue(),
      mkcert(),
  ]
})
