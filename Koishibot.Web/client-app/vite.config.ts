import { fileURLToPath, URL } from 'node:url';
import autoprefixer from 'autoprefixer';
import tailwind from 'tailwindcss';
import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue()],
  server: {
    port: 5210,
    strictPort: true,
    proxy: {
      '/notifications': {
        target: 'https://localhost:7115',
        changeOrigin: true,
        secure: false,
        ws: true
      },
      '/api': {
        target: 'https://localhost:7115/',
        changeOrigin: true,
        secure: false
      }
    }
  },
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  },
  css: {
    postcss: {
      plugins: [tailwind(), autoprefixer()]
    }
  },
  define: {
  __BUILD_TIMESTAMP__: JSON.stringify(new Date().toISOString()),
}
});