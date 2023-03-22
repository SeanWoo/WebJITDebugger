import Editor from 'react-simple-code-editor';
import { highlight, languages } from 'prismjs/components/prism-core';
import 'prismjs/components/prism-clike';
import 'prismjs/components/prism-csharp';
import 'prismjs/components/prism-javascript';
import 'prismjs/themes/prism.css'; //Example style, you can use another
import { StyledContainer, StyledEditor } from './styles';
import { useEffect, useState } from 'react';
import axios from 'axios';
import { Spin } from 'antd';

const INITIAL_CODE = `using System.Runtime.CompilerServices; 

public static class Program {
  [MethodImpl(MethodImplOptions.NoInlining)]
  public static int Test(int a, int b) {
    return a + b;
  }
  public static int Main(string[] args) {
    var result = Test(1, 2);
    return result;
  }
}

`;

const INITIAL_RESULT = "Нажмите кнопку 'Сохранить' для получения результата";

async function sendCode(code: string): Promise<string> {
  const response = await axios.post(
    '/api/disassembler',
    {
      code: code,
    },
    {
      validateStatus: () => true,
    },
  );

  if (response.status == 200) return response.data.assemblyCode;

  if (response.status == 400) return `Error type: ${response.data.type}\nMessage: ${response.data.message}`;

  return response.data.message;
}

export const EditorLayout = () => {
  const [code, setCode] = useState(INITIAL_CODE);
  const [result, setResult] = useState(INITIAL_RESULT);
  const [updateTimerId, setUpdateTimerId] = useState<NodeJS.Timeout>();

  useEffect(() => {
    (async () => {
      const response = await sendCode(code);
      setResult(response);
    })();
  }, []);

  const handleChangeCode = (code: string) => {
    setCode(code);

    clearTimeout(updateTimerId);
    const timerId = setTimeout(async () => {
      const response = await sendCode(code);
      setResult(response);
      setUpdateTimerId(undefined);
    }, 1000);
    setUpdateTimerId(timerId);
  };

  return (
    <StyledContainer>
      <StyledEditor
        value={code}
        onValueChange={handleChangeCode}
        highlight={(code) => highlight(code, languages.csharp)}
        padding={10}
      />
      <StyledEditor
        value={result}
        onValueChange={() => undefined}
        highlight={(code) => highlight(code, languages.text)}
        padding={10}
      />
      {updateTimerId && <Spin>Загрузка...</Spin>}
    </StyledContainer>
  );
};
