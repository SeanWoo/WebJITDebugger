declare module 'prismjs/components/prism-core' {
    const highlight: (args: any[]) => any;
    const languages: Map<string, any>;
    export { highlight, languages };
  }