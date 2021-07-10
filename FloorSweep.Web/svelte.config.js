/** @type {import('@sveltejs/kit').Config} */
import autoprefixer from 'autoprefixer';
import adapter from '@sveltejs/adapter-node'
import dotenv from "dotenv";
import  sveltePreprocess  from 'svelte-preprocess';
const cfg = dotenv.config();
const mode = process.env.NODE_ENV;
const dev = mode === 'development';

const preprocess = sveltePreprocess(
	{ sourceMap: dev ,
	scss: {
	includePaths: ['src']
	},postcss: {
		plugins: [autoprefixer],
	  }
});

const config = {
	// options passed to svelte.compile (https://svelte.dev/docs#svelte_compile)
	compilerOptions: {
		dev,
		hydratable: true
	},

	// an array of file extensions that should be treated as Svelte components
	extensions: ['.svelte'],

	kit: {
		adapter: adapter({
			// default options are shown
			out: 'build',
			precompress: false,
			env: {
				host: cfg.parsed.HOSTNAME,
				port: cfg.parsed.PORT
			}
		}),
		amp: false,
		appDir: '_app',
		files: {
			assets: 'static',
			hooks: 'src/hooks',
			lib: 'src/lib',
			routes: 'src/routes',
			serviceWorker: 'src/service-worker',
			template: 'src/app.html'
		},
		floc: false,
		host: cfg.parsed.HOSTNAME,
		hostHeader: cfg.parsed.HOSTNAME,
		hydrate: true,
		package: {
			dir: 'package',
			
			exports: {
				include: ['**'],
				exclude: ['_*', '**/_*']
			},
			files: {
				include: ['**'],
				exclude: []
			}
		},
		paths: {
			assets: '',
			base: ''
		},
		prerender: {
			crawl: true,
			enabled: true,
			force: false,
			pages: ['*']
		},
		router: true,
		serviceWorker: {
			exclude: []
		},
		ssr: true,
		target: "#app",
		trailingSlash: 'never',
		vite: () => ({})
	},

	// options passed to svelte.preprocess (https://svelte.dev/docs#svelte_preprocess)
	preprocess
};

export default config;