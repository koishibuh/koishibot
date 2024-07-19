/** @type {import('tailwindcss').Config} */
export default {
  content: ['./index.html', './src/**/*.{vue,js,ts,jsx,tsx}'],
  theme: {
    extend: {
      colors: {
        background: '#303031',
        primary: '#5e33b4',
        secondary: '#6a4cc8',
        'accent-one': '#f26c9e',
        'accent-two': '#48ace6',
        foreground: '#d6d6d6',
        b: '#5d87ff',
        g: '#007018'
      }
    }
  },
  plugins: []
};
