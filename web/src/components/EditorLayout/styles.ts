import styled from 'styled-components';
import Editor from 'react-simple-code-editor';

export const StyledContainer = styled.div`
  display: flex;
  gap: 12px;
`;

export const StyledEditor = styled(Editor)`
  background-color: #222;
  color: white;

  font-family: Consolas, Menlo, Monaco, monospace;
  line-height: 1.2;
  font-size: 14;

  width: 100%;
`;
